using AutoMapper;
using Google.Cloud.Vision.V1;
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
    private readonly ITagRepo _tagRepo;

    public PictureService(
        ILogger<PictureService> logger, 
        IAuthorizationService authorizationService, 
        IAccountContextService accountContextService,
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
        _pictureRepo = pictureRepo;
        _accountRepo = accountRepo;
        _likeRepo = likeRepo;
        _tagRepo = tagRepo;
    }
    
    public List<PictureDto> GetPictures(PictureQuery query)
    {
        List<PictureDto> pictureDtos;

        if (_accountContextService.TryGetAccountId() is not null)
        {
            var accId = _accountContextService.GetAccountId();
            var pictures = _pictureRepo.GetNotSeenByAccountId(accId, query.PageSize);

            var picArray = pictures.ToArray();
            foreach (var picture in picArray)
            {
                _accountRepo.MarkAsSeen(accId, picture.Id);
            }
            if (picArray.Any())
            {
                _accountRepo.Save();
            }
            pictureDtos = _mapper.Map<List<PictureDto>>(picArray).ToList();
        }
        else
        {
            var pictures = _pictureRepo.GetFromAll(
                query.PageSize * (query.PageNumber - 1),
                query.PageSize
            );
            pictureDtos = _mapper.Map<List<PictureDto>>(pictures).ToList();
        }
        return pictureDtos;
    }

    public PagedResult<PictureDto> SearchAll(SearchQuery query)
    {
        IEnumerable<Picture> pictures;

        switch (query.SearchBy)
        {
            case SortSearchBy.MostPopular:
                pictures = _pictureRepo.SearchAll(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    query.SearchPhrase);
                break;
            case SortSearchBy.Newest:
                pictures = _pictureRepo.SearchNewest(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    query.SearchPhrase);
                break;
            case SortSearchBy.MostLikes:
                pictures = _pictureRepo.SearchMostLikes(
                    query.PageSize * (query.PageNumber - 1),
                    query.PageSize,
                    query.SearchPhrase);
                break;
            default:
                throw new BadRequestException("Invalid 'search by' option");
        }

        var picBuffer = pictures as Picture[] ?? pictures.ToArray();
        if (!picBuffer.Any()) throw new NotFoundException();

        var resultCount = picBuffer.Length;
        var pictureDtos = _mapper
            .Map<List<PictureDto>>(pictures)
            .ToList();
        AllowModifyItems(pictureDtos);
        var result = new PagedResult<PictureDto>(pictureDtos, resultCount, query.PageSize, query.PageNumber);
        return result;
    }

    public  List<LikeDto> GetPicLikes(int id)
    {
        if (_pictureRepo.GetById(id) is null) throw new NotFoundException();
        var likes = _likeRepo.GetByLikedId(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);

        return likeDtos;
    }
    
    public  List<AccountDto> GetPicLikers(int id)
    {
        if (_pictureRepo.GetById(id) is null) throw new NotFoundException();
        var likes = _likeRepo.GetByLikedId(id);
        var accounts = likes.Select(like => like.Liker).ToList();
        var result = _mapper.Map<List<AccountDto>>(accounts);
        return result;
    }
    
    public PictureDto GetById(int id)
    {
        var picture = _pictureRepo.GetById(id);
        if (picture == null) throw new NotFoundException();
        var result = _mapper.Map<PictureDto>(picture);
        AllowModifyItems(result);
        return result;
    }
    
    public int Create(IFormFile file, CreatePictureDto dto)
    {
        var id = _accountContextService.GetAccountId();
        var account = _accountRepo.GetById(id);

        var picture = _mapper.Map<Picture>(dto);

        picture.Account = account;

        if (file is not { Length: > 0 }) throw new BadRequestException("invalid picture");
        
        var rootPath = Directory.GetCurrentDirectory();

        var randomName = $"{Path.GetRandomFileName().Replace('.', '-')}.webp";
        var fullPath = Path.Combine(rootPath, "wwwroot", "pictures", $"{randomName}");
        picture.Url = Path.Combine("wwwroot", "pictures", $"{randomName}");
        
        using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            file.CopyTo(stream);
            stream.Dispose();
        }
        _pictureRepo.Insert(picture);

        if (dto.Tags is not null)
        {
            foreach (var tag in dto.Tags.Distinct())
            {
                _tagRepo.InsertAndSave(new Tag()
                {
                    Value = tag,
                });
                _tagRepo.TryInsertPictureTagJoin(picture, _tagRepo.GetByValue(tag));
            }
        }

        _pictureRepo.Save();
        return picture.Id;

    }

    public SafeSearchAnnotation Classify(IFormFile file)
    {
        if (file is null) throw new BadRequestException("Invalid file");
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        return NsfwClassifier.ClassifyAsync(fileBytes).Result;
    }

    public PictureDto Update(int id, PutPictureDto dto)
    {
        var picture = _pictureRepo.GetById(id);
        if (picture is null) throw new NotFoundException();
        
        AuthorizePictureOperation(picture, ResourceOperation.Update,"you cant modify picture you didnt added");

        if (dto.Description is not null)
        {
            picture.Description = dto.Description;
        }
        if (dto.Name is not null)
        {
            picture.Name = dto.Name;
        }
        _pictureRepo.Update(picture);
        _pictureRepo.Save();
        var result = _mapper.Map<PictureDto>(picture);
        return result;
    }
        
    public void Delete(int id)
    {
        var picture = _pictureRepo.GetById(id);
        if (picture is null) throw new NotFoundException();
        _logger.LogWarning($"Picture with id: {id} DELETE action invoked");

        AuthorizePictureOperation(picture, ResourceOperation.Delete ,"you have no rights to delete this picture");

        _pictureRepo.DeleteById(picture.Id);
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), picture.Url);
        File.Delete(fullPath);
        _pictureRepo.Save();
        _logger.LogWarning($"Picture with id: {id} DELETE action success");
    }

    #region Private methods

    private void AllowModifyItems(List<PictureDto> items)
    {
        if (_accountContextService.TryGetAccountId() is null) return;
        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();
        foreach (var item in items)
        {
            if (item.AccountId == accountId)
            {
                item.IsModifiable = true;
            }
            else if (accountRole == 3)
            {
                item.IsAdminModifiable = true;
            }

            foreach (var comment in item.Comments)
            {
                if (comment.AccountId == accountId)
                {
                    comment.IsModifiable = true;
                }
                else if (accountRole == 3)
                {
                    item.IsAdminModifiable = true;
                }
            }

            if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
            {
                item.LikeState = LikeState.Liked;
            }
            if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
            {
                item.LikeState = LikeState.DisLiked;
            }
        }
    }

    private void AllowModifyItems(PictureDto item)
    {
        if (_accountContextService.TryGetAccountId() is null) return;
        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();
        if (item.AccountId == accountId)
        {
            item.IsModifiable = true;
        }
        else if (accountRole == 3)
        {
            item.IsAdminModifiable = true;
        }

        foreach (var comment in item.Comments)
        {
            if (comment.AccountId == accountId)
            {
                comment.IsModifiable = true;
            }
            else if (accountRole == 3)
            {
                item.IsAdminModifiable = true;
            }
        }

        if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
        {
            item.LikeState = LikeState.Liked;
        }
        if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
        {
            item.LikeState = LikeState.DisLiked;
        }
    }

    private void AuthorizePictureOperation(Picture picture, ResourceOperation operation, string message)
    {
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, picture, new PictureOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

    #endregion
    
}