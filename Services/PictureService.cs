using AutoMapper;
using FluentValidation;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Validators;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Helpers.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PictureService : IPictureService
{
    private readonly ILogger<PictureService> _logger;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;
    private readonly IModifyAllower _modifyAllower;
    private readonly IPictureRepo _pictureRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly ITagRepo _tagRepo;

    public PictureService(
        ILogger<PictureService> logger, 
        IAuthorizationService authorizationService, 
        IAccountContextService accountContextService,
        IModifyAllower modifyAllower,
        IPictureRepo pictureRepo,
        IAccountRepo accountRepo,
        ILikeRepo likeRepo,
        ITagRepo tagRepo,
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
        _modifyAllower = modifyAllower;
        _pictureRepo = pictureRepo;
        _accountRepo = accountRepo;
        _likeRepo = likeRepo;
        _tagRepo = tagRepo;
    }

    public async Task<IEnumerable<PictureDto>> GetPersonalizedPictures(PictureQueryPersonalized query)
    {
        var accId = _accountContextService.GetAccountId();
        var pictures = await _pictureRepo.GetNotSeenByAccountIdAsync(accId, query.PageSize);

        var picArray = pictures.ToArray(); // avoiding multiple enumeration
        foreach (var picture in picArray)
        {
            await _accountRepo.MarkAsSeenAsync(accId, picture.Id);
        }
        var pictureDtos = _mapper.Map<List<PictureDto>>(picArray).ToList();
        _modifyAllower.UpdateItems(pictureDtos);
        return pictureDtos;
    }

    public async Task<PagedResult<PictureDto>> GetPictures(PictureQuery query)
    {
        var pictures = await _pictureRepo.GetFromAllAsync(
            query.PageSize * (query.PageNumber - 1),
            query.PageSize
        );
        var pictureDtos = _mapper.Map<List<PictureDto>>(pictures);
        _modifyAllower.UpdateItems(pictureDtos);
        return new PagedResult<PictureDto>(
            pictureDtos,
            query.PageSize,
            query.PageNumber,
            await _pictureRepo.CountPicturesAsync((p) => true)
        );
    }

    public async Task<PagedResult<PictureDto>> SearchAll(SearchQuery query)
    {
        IEnumerable<Picture> pictures;

        switch (query.SearchBy)
        {
            case SortSearchBy.MostPopular:
                pictures = await _pictureRepo.SearchAllAsync(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    query.SearchPhrase);
                break;
            case SortSearchBy.Newest:
                pictures = await _pictureRepo.SearchNewestAsync(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    query.SearchPhrase);
                break;
            case SortSearchBy.MostLikes:
                pictures = await _pictureRepo.SearchMostLikesAsync(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    query.SearchPhrase);
                break;
            default:
                throw new BadRequestException("Invalid 'search by' option");
        }

        var picBuffer = pictures as Picture[] ?? pictures.ToArray();
        if (!picBuffer.Any()) throw new NotFoundException();

        var pictureDtos = _mapper
            .Map<List<PictureDto>>(pictures)
            .ToList();
        _modifyAllower.UpdateItems(pictureDtos);
        var result = new PagedResult<PictureDto>(
            pictureDtos,
            query.PageSize,
            query.PageNumber,
            await _pictureRepo.CountPicturesAsync(
                p => query.SearchPhrase == string.Empty || p.Name.ToLower().Contains(query.SearchPhrase.ToLower())
            ));
        return result;
    }

    public async Task<IEnumerable<LikeDto>> GetPicLikes(int id)
    {
        if (await _pictureRepo.GetByIdAsync(id) is null) throw new NotFoundException();
        var likes = await _likeRepo.GetByLikedIdAsync(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);

        return likeDtos;
    }
    
    public async Task<IEnumerable<AccountDto>> GetPicLikers(int id)
    {
        if (await _pictureRepo.GetByIdAsync(id) is null) throw new NotFoundException();
        var likes = await _likeRepo.GetByLikedIdAsync(id);
        var accounts = likes.Select(like => like.Liker).ToList();
        var accountDtos = _mapper.Map<List<AccountDto>>(accounts);
        _modifyAllower.UpdateItems(accountDtos);
        return accountDtos;
    }
    
    public async Task<PictureDto> GetById(int id)
    {
        var picture = await _pictureRepo.GetByIdAsync(id);
        if (picture == null) throw new NotFoundException();
        var pictureDtos = _mapper.Map<PictureDto>(picture);
        _modifyAllower.UpdateItems(pictureDtos);
        return pictureDtos;
    }
    
    public async Task<string> Create(IFormFile file, CreatePictureDto dto)
    {
        var validationResult = await StaticValidator.Validate(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var account = await _accountContextService.GetAccountAsync();
        var picture = _mapper.Map<Picture>(dto);

        picture.Account = account;

        if (file is not { Length: > 0 }) throw new BadRequestException("invalid picture");
        
        var rootPath = Directory.GetCurrentDirectory();

        var randomName = $"{Path.GetRandomFileName().Replace('.', '-')}.webp";
        var fullPath = Path.Combine(rootPath, "wwwroot", "pictures", $"{randomName}");
        picture.Url = Path.Combine("wwwroot", "pictures", $"{randomName}");

        try
        {
            await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();
            }
            await _pictureRepo.InsertAsync(picture);
            if (dto.Tags is not null)
            {
                foreach (var tag in dto.Tags.Distinct())
                {
                    var insertedTag = await _tagRepo.InsertAsync(new Tag()
                    {
                        Value = tag,
                    });
                    await _tagRepo.TryInsertPictureTagJoinAsync(picture, insertedTag);
                }
            }
            return IdHasher.EncodePictureId(picture.Id);
        }
        catch (Exception)
        {
            if (File.Exists(fullPath)) File.Delete(fullPath);
            throw;
        }
    }

    public async Task<SafeSearchAnnotation> Classify(IFormFile file)
    {
        if (file is null) throw new BadRequestException("Invalid file");
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var fileBytes = ms.ToArray();
        return await NsfwClassifier.ClassifyAsync(fileBytes, CancellationToken.None);
    }

    public async Task<PictureDto> Update(int id, UpdatePictureDto dto)
    {
        var validationResult = await StaticValidator.Validate(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var picture = await _pictureRepo.GetByIdAsync(id);
        if (picture is null) throw new NotFoundException();
        
        await AuthorizePictureOperation(picture, ResourceOperation.Update,"you cant modify picture you didnt added");

        if (dto.Description is not null) picture.Description = dto.Description;
        if (dto.Name is not null) picture.Name = dto.Name;
        if (dto.Tags is not null && dto.Tags.Count > 0)
        {
            foreach (var join in picture.PictureTagJoins)
            {
                await _tagRepo.TryDeletePictureTagJoinAsync(join.Picture, join.Tag);
            }
            foreach (var tag in dto.Tags)
            {
                var insertedTag = await _tagRepo.InsertAsync(new Tag() { Value = tag });
                await _tagRepo.TryInsertPictureTagJoinAsync(picture, insertedTag);
            }
        }

        await _pictureRepo.UpdateAsync(picture);
        var pictureDto = _mapper.Map<PictureDto>(picture);
        _modifyAllower.UpdateItems(pictureDto);
        return pictureDto;
    }
        
    public async Task<bool> Delete(int id)
    {
        var picture = await _pictureRepo.GetByIdAsync(id) ?? throw new NotFoundException();
        _logger.LogWarning($"Picture with id: {id} DELETE action invoked");

        await AuthorizePictureOperation(picture, ResourceOperation.Delete ,"you have no rights to delete this picture");

        try
        {
            await _pictureRepo.DeleteByIdAsync(picture.Id);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), picture.Url);
            File.Delete(fullPath);
            _logger.LogWarning($"Picture with id: {id} DELETE action success");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #region Private methods

    private async Task AuthorizePictureOperation(Picture picture, ResourceOperation operation, string message)
    {
        var user = _accountContextService.User;
        var authorizationResult = await _authorizationService.AuthorizeAsync(user, picture, new PictureOperationRequirement(operation));
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

    #endregion
    
}