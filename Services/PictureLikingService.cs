using System;
using System.Linq;
using System.Security.Claims;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

// use separate folders for these


// change this whole bitch to factory
public class PictureLikingService : IPictureLikingService
{
    private readonly IPictureRepo _pictureRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeRepo likeRepo,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _likeRepo = likeRepo;
        _accountRepo = accountRepo;
        _accountContextService = accountContextService;
    }
    
    public LikeOperationResult Like(Guid id)
    {
        var user = _accountContextService.User;
        var picture = _pictureRepo.GetPictureById(id);
        
        if (picture is null) throw new NotFoundException("picture not found");
        
        var accountId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var account = _accountRepo.GetAccountById(Guid.Parse(accountId));
        
        if (account is null) throw new InvalidAuthTokenException();
        
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
                return LikeOperationResult.LikeRemoved;
            }
            else
            {
                // is dislike
                _accountRepo.AddLikedTags(account, picture);
                _likeRepo.ChangeLike(like);
                return LikeOperationResult.Liked;
            }
        }
        else
        {
            // like does not exist
            _accountRepo.AddLikedTags(account, picture);
            _likeRepo.AddLike(
                new Like()
                {
                    Liked = picture,
                    Liker = account,
                    IsLike = true
                });
            return LikeOperationResult.Liked;
        }
    }

    public LikeOperationResult DisLike(Guid id)
    {
        var user = _accountContextService.User;
        var picture = _pictureRepo.GetPictureById(id);
        
        if (picture is null) throw new NotFoundException("picture not found");
        
        var accountId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var account = _accountRepo.GetAccountById(Guid.Parse(accountId));
        
        if (account is null) throw new InvalidAuthTokenException();
        
        var like = _likeRepo.GetLikeByLikerAndLiked(account, picture);

        // If we call dislike
        if (like is not null)
        {
            // dislike exists
            if (like.IsLike == false)
            {
                // is dislike
                _likeRepo.RemoveLike(like);
                return LikeOperationResult.DislikeRemoved;
            }
            else
            {
                // is like
                _likeRepo.ChangeLike(like);
                return LikeOperationResult.Disliked;
            }
        }
        else
        {
            // dislike does not exist
            _likeRepo.AddLike(
                new Like()
                {
                    Liked = picture,
                    Liker = account,
                    IsLike = false
                });
            return LikeOperationResult.Disliked;
            
        }
        
    }

}