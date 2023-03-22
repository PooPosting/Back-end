using AutoMapper;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Authorization;
using PooPosting.Api.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Enums;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Helpers.Interfaces;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services;

public class PictureService : IPictureService
{
    private readonly ILogger<PictureService> _logger;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;
    private readonly IPictureRepo _pictureRepo;
    private readonly IPictureHelper _pictureHelper;
    private readonly ITagRepo _tagRepo;
    private readonly ITagHelper _tagHelper;

    public PictureService(
        ILogger<PictureService> logger,
        IAuthorizationService authorizationService,
        IAccountContextService accountContextService,
        IPictureRepo pictureRepo,
        IPictureHelper pictureHelper,
        ITagRepo tagRepo,
        ITagHelper tagHelper,
        IMapper mapper
        )
    {
        _logger = logger;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
        _pictureRepo = pictureRepo;
        _pictureHelper = pictureHelper;
        _tagRepo = tagRepo;
        _tagHelper = tagHelper;
    }

    public async Task<PictureDto> GetById(
        int id
    )
    {
        var picture = await _pictureRepo.GetByIdAsync(id);
        if (picture == null) throw new NotFoundException();
        var pictureDtos = _mapper.Map<PictureDto>(picture);
        return pictureDtos;
    }

    public async Task<IEnumerable<PictureDto>> GetPersonalizedPictures(
        PersonalizedQuery query
        )
    {
        var accId = _accountContextService.GetAccountId();
        var pictures = await _pictureRepo.GetNotSeenByAccountIdAsync(accId, query.PageSize);

        var picArray = pictures.ToList();
        foreach (var picture in picArray)
        {
            await _pictureHelper.MarkAsSeenAsync(accId, picture.Id);
        }
        var pictureDtos = _mapper.Map<List<PictureDto>>(picArray).ToList();
        return pictureDtos;
    }

    public async Task<PagedResult<PictureDto>> GetPictures(
        Query query
        )
    {
        var pictures = await _pictureRepo.SearchAllAsync(
            query.PageSize * (query.PageNumber - 1),
            query.PageSize,
            p => p.PopularityScore,
            null
        );
        var pictureDtos = _mapper.Map<List<PictureDto>>(pictures);
        return new PagedResult<PictureDto>(
            pictureDtos,
            query.PageNumber,
            query.PageSize,
            await _pictureRepo.CountPicturesAsync()
        );
    }
    public async Task<PagedResult<PicturePreviewDto>> GetLikedPictures(
        int accId,
        Query query
        )
    {
        var pictures = await _pictureRepo.GetLikedPicturesByAccountIdAsync(
            accId,
            query.PageSize * (query.PageNumber - 1),
            query.PageSize
        );

        var picPreviews = _mapper.Map<IEnumerable<PicturePreviewDto>>(pictures);

        var result = new PagedResult<PicturePreviewDto>(
            picPreviews,
            query.PageNumber,
            query.PageSize,
            await _pictureRepo.CountPicturesAsync(p => p.AccountId == accId)
        );

        return result;
    }
    public async Task<PagedResult<PicturePreviewDto>> GetPostedPictures(
        int accId,
        Query query
        )
    {
        var pictures = await _pictureRepo.GetPostedPicturesByAccountIdAsync(
            accId,
            query.PageSize * (query.PageNumber - 1),
            query.PageSize
        );

        var picPreviews = _mapper.Map<IEnumerable<PicturePreviewDto>>(pictures);

        var result = new PagedResult<PicturePreviewDto>(
            picPreviews,
            query.PageNumber,
            query.PageSize,
            await _pictureRepo.CountPicturesAsync(p => p.AccountId == accId)
        );

        return result;
    }

    public async Task<PagedResult<PictureDto>> SearchAll(
        CustomQuery query
        )
    {
        IEnumerable<Picture> pictures;

        switch (query.SearchBy)
        {
            case SortBy.MostPopular:
                pictures = await _pictureRepo.SearchAllAsync(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    p => p.PopularityScore,
                    p => query.SearchPhrase == string.Empty || p.Name.ToLower().Contains(query.SearchPhrase.ToLower()));
                break;
            case SortBy.Newest:
                pictures = await _pictureRepo.SearchAllAsync(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    p => p.PictureAdded.Ticks,
                    p => query.SearchPhrase == string.Empty || p.Name.ToLower().Contains(query.SearchPhrase.ToLower()));
                break;
            case SortBy.MostLikes:
                pictures = await _pictureRepo.SearchAllAsync(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    p => p.Likes.Count(l => l.IsLike),
                    p => query.SearchPhrase == string.Empty || p.Name.ToLower().Contains(query.SearchPhrase.ToLower()));
                break;
            default:
                pictures = await _pictureRepo.SearchAllAsync(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    p => p.PopularityScore,
                    p => query.SearchPhrase == string.Empty || p.Name.ToLower().Contains(query.SearchPhrase.ToLower()));
                break;
        }

        var picBuffer = pictures as List<Picture> ?? pictures.ToList();
        if (!picBuffer.Any()) throw new NotFoundException();

        var pictureDtos = _mapper
            .Map<List<PictureDto>>(pictures)
            .ToList();
        var result = new PagedResult<PictureDto>(
            pictureDtos,
            query.PageNumber,
            query.PageSize,
            await _pictureRepo.CountPicturesAsync(p => query.SearchPhrase == string.Empty || p.Name.ToLower().Contains(query.SearchPhrase.ToLower()))
            );
        return result;
    }

    public async Task<PictureDto> UpdatePictureName(
        int picId,
        UpdatePictureNameDto dto
        )
    {
        var picture = await _pictureRepo.GetByIdAsync(picId);
        if (picture is null) throw new NotFoundException();
        await AuthorizePictureOperation(picture, ResourceOperation.Update, "you cannot modify picture you didnt post");
        picture.Name = dto.Name;
        return _mapper.Map<PictureDto>(await _pictureRepo.UpdateAsync(picture));
    }

    public async Task<PictureDto> UpdatePictureDescription(
        int picId,
        UpdatePictureDescriptionDto dto
    )
    {
        var picture = await _pictureRepo.GetByIdAsync(picId);
        if (picture is null) throw new NotFoundException();
        await AuthorizePictureOperation(picture, ResourceOperation.Update, "you cannot modify picture you didnt post");
        picture.Description = dto.Description;
        return _mapper.Map<PictureDto>(await _pictureRepo.UpdateAsync(picture));
    }

    public async Task<PictureDto> UpdatePictureTags(
        int picId,
        UpdatePictureTagsDto dto
    )
    {
        var picture = await _pictureRepo.GetByIdAsync(picId);
        if (picture is null) throw new NotFoundException();
        await AuthorizePictureOperation(picture, ResourceOperation.Update, "you cannot modify picture you didnt post");
        await _tagHelper.TryUpdatePictureTagsAsync(picture, dto.Tags);

        return _mapper.Map<PictureDto>(await _pictureRepo.UpdateAsync(picture));
    }

    public async Task<string> Create(
        CreatePictureDto dto
        )
    {
        if (dto.File is not { Length: > 0 }) throw new BadRequestException("invalid picture");

        using var ms = new MemoryStream();
        await dto.File.CopyToAsync(ms);
        var fileBytes = ms.ToArray();
        var result = await NsfwClassifier.ClassifyAsync(fileBytes, CancellationToken.None);

        var errors = new List<string>();

        if (result.Adult > Likelihood.Possible) errors.Add("Adult");
        if (result.Racy > Likelihood.Likely) errors.Add("Racy");
        if (result.Medical > Likelihood.Likely) errors.Add("Medical");
        if (result.Violence > Likelihood.Likely) errors.Add("Violence");

        if (errors.Any())
        {
            throw new BadRequestException($"inappropriate picture: [{string.Join(", ", errors)}]");
        }

        var accountId = _accountContextService.GetAccountId();
        var picture = _mapper.Map<Picture>(dto);

        picture.AccountId = accountId;

        var rootPath = Directory.GetCurrentDirectory();

        var randomName = $"{Path.GetRandomFileName().Replace('.', '-')}.webp";
        var fullPath = Path.Combine(rootPath, "wwwroot", "pictures", $"{randomName}");
        picture.Url = Path.Combine("wwwroot", "pictures", $"{randomName}");

        try
        {
            await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                await dto.File.CopyToAsync(stream);
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
                    await _tagHelper.TryInsertPictureTagJoinAsync(picture, insertedTag);
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

    public async Task<bool> Delete(
        int id
        )
    {
        var picture = await _pictureRepo.GetByIdAsync(id) ?? throw new NotFoundException();
        _logger.LogWarning($"Picture with id: {id} DELETE (hide) action invoked");

        await AuthorizePictureOperation(picture, ResourceOperation.Delete ,"you have no rights to delete this picture");

        try
        {
            picture.IsDeleted = true;
            await _pictureRepo.UpdateAsync(picture);
            _logger.LogWarning($"Picture with id: {id} DELETE (hide) action success");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #region Private methods

    private async Task AuthorizePictureOperation(
        Picture picture,
        ResourceOperation operation,
        string message
        )
    {
        var user = _accountContextService.User;
        var authorizationResult = await _authorizationService.AuthorizeAsync(user, picture, new PictureOperationRequirement(operation));
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

    #endregion
    
}