using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
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

    public PictureService(
        ILogger<PictureService> logger, 
        IAuthorizationService authorizationService, 
        IAccountContextService accountContextService,
        IPictureRepo pictureRepo,
        IAccountRepo accountRepo,
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
        _pictureRepo = pictureRepo;
        _accountRepo = accountRepo;
    }
    
    public PagedResult<PictureDto> GetAll(PictureQuery query)
    {
        var baseQuery = _pictureRepo.GetPictures() // how to make this less painful for the database?
            .Where(p => query.SearchPhrase == null || 
                        p.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || 
                        p.Tags.ToLower().Contains(query.SearchPhrase.ToLower()))
            .Where(p => p.PictureAdded.AddDays(query.DaysSincePictureAdded) > DateTime.Now)
            .OrderByDescending(p => p.Tags.Split(' ').Intersect(query.LikedTags.Split(' ')).Count())
            .ThenByDescending(p => p.Likes.Count)
            .ToList();

        var pictures = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();
        
        if (pictures.Count == 0) throw new NotFoundException("pictures not found");
        
        var resultCount = baseQuery.Count;
        var pictureDtos = _mapper.Map<List<PictureDto>>(pictures).ToList();
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
        var account = _accountRepo.GetAccountById(Guid.Parse(id));
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
            
            var result = _pictureRepo.CreatePicture(picture);
            
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return result;
        }

        throw new BadRequestException("invalid picture");

    }

    public bool Put(Guid id, PutPictureDto dto)
    {
        var picture = _pictureRepo.GetPictureById(id);
        if (picture is null) throw new NotFoundException("picture not found");
        var user = _accountContextService.User;

        var authorizationResult = _authorizationService.AuthorizeAsync(user, picture, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException("You can't modify picture you didn't added");

        var result = _pictureRepo.UpdatePicture(picture, dto);
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
        
        _logger.LogWarning($"Picture with id: {id} DELETE action success");

        return pictureDeleteResult;
    }
    
    
}