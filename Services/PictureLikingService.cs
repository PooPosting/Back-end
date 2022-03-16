using System.Security.Claims;
using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PictureLikingService : IPictureLikingService
{
    private readonly IPictureRepo _pictureRepo;
    private readonly IMapper _mapper;
    private readonly ILikeRepo _likeRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeRepo likeRepo,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        IMapper mapper,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _mapper = mapper;
        _likeRepo = likeRepo;
        _accountRepo = accountRepo;
        _accountContextService = accountContextService;
    }
    
    public PictureDto Like(Guid id)
    {
        var picture = _pictureRepo.GetPictureById(id);
        var accountId = _accountContextService.GetAccountId;

        if (picture is null) throw new NotFoundException("picture not found");
        if (accountId is null) throw new ForbidException("please log in");
        if (!_accountRepo.Exists(Guid.Parse(accountId))) throw new InvalidAuthTokenException();
        
        var account = _accountRepo.GetAccountById(Guid.Parse(accountId), DbInclude.Raw);
        var like = _likeRepo.GetLikeByLikerAndLiked(account, picture);
        
        // If we call like
        if (like is not null)
        {
            // like exists
            if (like.IsLike)
            {
                // is like
                _accountRepo.RemoveLikedTags(account, picture);
                _likeRepo.RemoveLike(like);
                return _mapper.Map<PictureDto>(picture);
            }
            // is dislike
            _accountRepo.AddLikedTags(account, picture);
            _likeRepo.ChangeLike(like);
            return _mapper.Map<PictureDto>(picture);
        }
        // like does not exist
        _accountRepo.AddLikedTags(account, picture);
        _likeRepo.AddLike(
            new Like()
            {
                Liked = picture,
                Liker = account,
                IsLike = true
            });
        return _mapper.Map<PictureDto>(picture);

    }

    public PictureDto DisLike(Guid id)
    {
        var picture = _pictureRepo.GetPictureById(id);
        var accountId = _accountContextService.GetAccountId;

        if (picture is null) throw new NotFoundException("picture not found");
        if (accountId is null) throw new ForbidException("please log in");
        if (!_accountRepo.Exists(Guid.Parse(accountId))) throw new InvalidAuthTokenException();
        
        var account = _accountRepo.GetAccountById(Guid.Parse(accountId), DbInclude.Raw);
        var like = _likeRepo.GetLikeByLikerAndLiked(account, picture);
        
        
        // If we call dislike
        if (like is not null)
        {
            // dislike exists
            if (like.IsLike == false)
            {
                // is dislike
                _likeRepo.RemoveLike(like);
                return _mapper.Map<PictureDto>(picture);
            }
            // is like
            _likeRepo.ChangeLike(like);
            return _mapper.Map<PictureDto>(picture);
        } 
        // dislike does not exist
        _likeRepo.AddLike(
            new Like()
            {
                Liked = picture,
                Liker = account,
                IsLike = false
            });
        return _mapper.Map<PictureDto>(picture);
    }

}