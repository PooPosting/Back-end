using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace PicturesAPI.Services;

public class PictureService : IPictureService
{
    private readonly ILogger<PictureService> _logger;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;
    private readonly IPictureRepo _pictureRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly IClassifyNsfw _classifyNsfw;

    public PictureService(
        ILogger<PictureService> logger, 
        IAuthorizationService authorizationService, 
        IAccountContextService accountContextService,
        IPictureRepo pictureRepo,
        IAccountRepo accountRepo,
        ILikeRepo likeRepo,
        IClassifyNsfw classifyNsfw,
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
        _pictureRepo = pictureRepo;
        _accountRepo = accountRepo;
        _likeRepo = likeRepo;
        _classifyNsfw = classifyNsfw;
    }
    
    public PagedResult<PictureDto> GetAll(PictureQuery query)
    {
        var baseQuery = _pictureRepo
            .GetPictures()
            .ToList();
        
        var sortedQuery = SortPictures
            .SortPics(baseQuery, query)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize);
        
        var pictures = sortedQuery
            .Select(picture => _pictureRepo.GetPictureById(picture.Id))
            .ToList();

        if (pictures.Count == 0) throw new NotFoundException("pictures not found");
        
        var resultCount = baseQuery.Count;
        var pictureDtos = _mapper.Map<List<PictureDto>>(pictures).ToList();
        
        AllowModify(pictureDtos);
            
        var result = new PagedResult<PictureDto>(pictureDtos, resultCount, query.PageSize, query.PageNumber);
        return result;
    }

    public PagedResult<PictureDto> SearchAll(SearchQuery query)
    {
        var baseQuery = _pictureRepo.GetPictures().ToList();

        List<Picture> sortedQuery;

        switch (query.SearchBy)
        {
            case SortSearchBy.MostPopular:
                sortedQuery = baseQuery
                    .OrderByDescending(p => p.Likes.Count)
                    .ToList();
                break;
            case SortSearchBy.Newest:
                sortedQuery = baseQuery
                    .OrderByDescending(p => p.PictureAdded)
                    .ToList();
                break;
            case SortSearchBy.MostLikes:
                sortedQuery = baseQuery
                    .OrderByDescending(p => p.Likes.Count(l => l.IsLike))
                    .ToList();
                break;
            default:
                throw new BadRequestException("Invalid 'search by' option");
        }

        var pictures = sortedQuery
            .Where(p => query.SearchPhrase == null ||
                        p.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                        p.Tags.ToLower().Contains(query.SearchPhrase.ToLower()))
            .Select(picture => _pictureRepo.GetPictureById(picture.Id))
            .ToList();

        if (pictures.Count == 0) throw new NotFoundException("pictures not found");
        
        var resultCount = pictures.Count;
        var pictureDtos = _mapper
            .Map<List<PictureDto>>(pictures)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();
        
        AllowModify(pictureDtos);

        var result = new PagedResult<PictureDto>(pictureDtos, resultCount, query.PageSize, query.PageNumber);
        return result;
    }

    public IEnumerable<PictureDto> GetAllOdata()
    {
        var pictures = _pictureRepo.GetPictures().ToList();
        if (pictures.Count == 0) throw new NotFoundException("pictures not found");
        var result = _mapper.Map<List<PictureDto>>(pictures);
        
        return result;
    }

    public List<LikeDto> GetPicLikes(Guid id)
    {
        var likes = _likeRepo.GetLikesByLiked(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);

        return likeDtos;
    }

    public PictureDto GetById(Guid id)
    {
        var picture = _pictureRepo.GetPictureById(id);
        if (picture == null) throw new NotFoundException("picture not found");
        var result = _mapper.Map<PictureDto>(picture);
        return result;
    }
    
    public Guid Create(IFormFile file, CreatePictureDto dto)
    {
        var id = _accountContextService.GetAccountId!;
        var account = _accountRepo.GetAccountById(Guid.Parse(id), DbInclude.Raw);
        if (account is null || account.IsDeleted) throw new InvalidAuthTokenException();
        
        dto.Tags = dto.Tags.Distinct().ToList();
        var picture = _mapper.Map<Picture>(dto);
        picture.Account = account;

        if (file is { Length: > 0 })
        {
            var rootPath = Directory.GetCurrentDirectory();
            var fileGuid = Guid.NewGuid();
            var fullPath = $"{rootPath}/wwwroot/pictures/{fileGuid}.webp";
            picture.Id = fileGuid;
            picture.Url = $"wwwroot/pictures/{fileGuid}.webp";


            using (var stream =
                   new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                file.CopyTo(stream);
                stream.Dispose();
            }

            var isSafe = _classifyNsfw.IsSafeForWork(fileGuid.ToString());
            if (!isSafe)
            {
                File.Delete(fullPath);
                throw new BadRequestException("nsfw picture");
            }
            var result = _pictureRepo.CreatePicture(picture);
            return result;

        }

        throw new BadRequestException("invalid picture");

    }

    public PictureDto Put(Guid id, PutPictureDto dto)
    {
        var picture = _pictureRepo.GetPictureById(id);
        if (picture is null) throw new NotFoundException("picture not found");
        var user = _accountContextService.User;

        var authorizationResult = _authorizationService.AuthorizeAsync(user, picture, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException("You can't modify picture you didn't added");
        _pictureRepo.UpdatePicture(picture, dto);

        var result = _mapper.Map<PictureDto>(picture);
        return result;
    }
        
    public bool Delete(Guid id)
    {
        var picture = _pictureRepo.GetPictureById(id);
        if (picture is null) throw new NotFoundException("picture not found");
        var user = _accountContextService.User;

        _logger.LogWarning($"Picture with id: {id} DELETE action invoked");
        var authorizationResult = _authorizationService.AuthorizeAsync(user, picture, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException("You have no rights to delete this picture");

        var pictureDeleteResult = _pictureRepo.DeletePicture(picture);

        if (pictureDeleteResult)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var fullPath = $"{rootPath}/wwwroot/pictures/{picture.Id}.webp";
            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        
        _logger.LogWarning($"Picture with id: {id} DELETE action success");

        return pictureDeleteResult;
    }
    
    private void AllowModify(List<PictureDto> pictureDtos)
    {
        var role = _accountContextService.GetAccountRole;
        var accountId = _accountContextService.GetAccountId;

        switch (role)
        {
            case ("3"):
                pictureDtos.ForEach(p => p.IsModifiable = true);
                break;
            default:
                pictureDtos
                    .Where(p => p.AccountId.ToString() == accountId)
                    .ToList()
                    .ForEach(p => p.IsModifiable = true);
                break;
        }
    }
    
}