using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Interfaces;

namespace PicturesAPI.Services;

public enum LikeOperationResult
{
    Liked,
    LikeRemoved,
    Disliked,
    DislikeRemoved
}
public class PictureLikingService : IPictureLikingService
{
    private readonly PictureDbContext _dbContext;
    private readonly ILogger<PictureLikingService> _logger;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(PictureDbContext dbContext, ILogger<PictureLikingService> logger, IAccountContextService accountContextService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _accountContextService = accountContextService;
    }
    
    public LikeOperationResult Like(Guid id)
    {
        var user = _accountContextService.User;
        var picture = _dbContext.Pictures
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .SingleOrDefault(p => p.Id == id);
        
        if (picture is null) throw new NotFoundException("Picture not found");
        
        var accountId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id.ToString() == accountId);
        if (account is null) throw new InvalidAuthTokenException();
        
        if (_dbContext.Likes.Any(l => l.Liker == account && l.Liked == picture))
        {
            var result = RemoveLike(picture, account);
            return result;
        }
        else
        {
            var result = AddLike(picture, account);
            return result;
        }
    }

    public LikeOperationResult DisLike(Guid id)
    {
        var user = _accountContextService.User;
        var picture = _dbContext.Pictures
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .SingleOrDefault(p => p.Id == id);
        
        if (picture is null) throw new NotFoundException("Picture not found");
        
        var accountId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id.ToString() == accountId);
        if (account is null) throw new InvalidAuthTokenException();

        if (_dbContext.Dislikes.Any(l => l.DisLiker == account && l.DisLiked == picture))
        {
            var result = RemoveDisLike(picture, account);
            return result;
        }
        else
        {
            var result = AddDisLike(picture, account);
            return result;
        }
        
    }

    // PRIVATE METHODS | PRIVATE METHODS | PRIVATE METHODS | PRIVATE METHODS | PRIVATE METHODS | PRIVATE METHODS 
    
    private LikeOperationResult RemoveLike(Picture picture, Account account)
    {
        var likeToRemove = _dbContext.Likes.SingleOrDefault(l => l.Liked == picture && l.Liker == account);
        account.LikedTags ??= " ";
        var pictureTags = picture.Tags.Split(' ').ToList();
        var likedTags = account.LikedTags.Split(' ').ToList();
        
        for (int i = 0; i < 3; i++) 
        {
            likedTags.Remove(pictureTags[i]);
        }

        account.LikedTags = string.Join(' ', likedTags);

        _dbContext.Likes.Remove(likeToRemove!);
        _dbContext.SaveChanges();
        return LikeOperationResult.Liked;
    }

    private LikeOperationResult AddLike(Picture picture, Account account)
    {
        var disLikeToRemove = _dbContext.Dislikes.SingleOrDefault(l => l.DisLiked == picture && l.DisLiker == account);
        if (disLikeToRemove is not null) _dbContext.Dislikes.Remove(disLikeToRemove!);
        
        var pictureTags = picture.Tags.ToLower().Split(' ').ToList();
        account.LikedTags ??= " ";
        var likedTags = account.LikedTags.ToLower().Split(' ').ToList();
        likedTags = likedTags.Distinct().ToList();
        
        foreach (var tag in likedTags)
        {
            if (pictureTags.Any(t => t == tag))
            {
                pictureTags.Remove(tag);
            }
        }
        likedTags.AddRange(pictureTags.Take(3));
        
        if (likedTags.Count > 30)
        {
            likedTags.RemoveRange(30, likedTags.Count - 30);
        }
        
        account.LikedTags = string.Join(' ', likedTags);
        _dbContext.Likes.Add(
            new Like()
            {
                Liked = picture,
                Liker = account
            });
        _dbContext.SaveChanges();
        return LikeOperationResult.LikeRemoved;
    }

    private LikeOperationResult RemoveDisLike(Picture picture, Account account)
    {
        var disLikeToRemove = _dbContext.Dislikes.SingleOrDefault(l => l.DisLiked == picture && l.DisLiker == account);
        _dbContext.Dislikes.Remove(disLikeToRemove!);
        _dbContext.SaveChanges();
        return LikeOperationResult.DislikeRemoved;
    }

    private LikeOperationResult AddDisLike(Picture picture, Account account)
    {
        var likeToRemove = _dbContext.Likes.SingleOrDefault(l => l.Liked == picture && l.Liker == account);
        if (likeToRemove is not null) _dbContext.Likes.Remove(likeToRemove);
        _dbContext.Dislikes.Add(
            new Dislike()
            {
                DisLiked = picture,
                DisLiker = account
            });
        _dbContext.SaveChanges();
        return LikeOperationResult.Disliked;
    }
}