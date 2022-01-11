using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    
    public LikeOperationResult LikePicture(Guid id)
    {
        var user = _accountContextService.User;
        var picture = _dbContext.Pictures
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .SingleOrDefault(p => p.Id == id);
        
        if (picture is null) throw new NotFoundException("Picture not found");
        
        var accountId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id.ToString() == accountId);
        if (account is null) throw new InvalidAuthTokenException();
        
        if (_dbContext.Likes.Any(l => l.Liker == account && l.Liked == picture))
        {
            var LikeToRemove = _dbContext.Likes.SingleOrDefault(l => l.Liked == picture && l.Liker == account);
            _dbContext.Likes.Remove(LikeToRemove!);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Picture {picture.Name} is not liked by {account.Nickname} anymore");
            return LikeOperationResult.Liked;
        }
        else
        {
            var DisLikeToRemove = _dbContext.Dislikes.SingleOrDefault(l => l.DisLiked == picture && l.DisLiker == account);
            if (DisLikeToRemove is not null) _dbContext.Dislikes.Remove(DisLikeToRemove!);
            _dbContext.Likes.Add(
                new Like()
                {
                    Liked = picture,
                    Liker = account
                });
            _dbContext.SaveChanges();
            _logger.LogInformation($"Picture {picture.Name} has been liked by {account.Nickname}");
            return LikeOperationResult.LikeRemoved;
        }
    }

    public LikeOperationResult DisLikePicture(Guid id)
    {
        var user = _accountContextService.User;
        var picture = _dbContext.Pictures
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .SingleOrDefault(p => p.Id == id);
        
        if (picture is null) throw new NotFoundException("Picture not found");
        
        var accountId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id.ToString() == accountId);
        if (account is null) throw new InvalidAuthTokenException();

        if (_dbContext.Dislikes.Any(l => l.DisLiker == account && l.DisLiked == picture))
        {
            var DisLikeToRemove = _dbContext.Dislikes.SingleOrDefault(l => l.DisLiked == picture && l.DisLiker == account);
            _dbContext.Dislikes.Remove(DisLikeToRemove!);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Picture {picture.Name} is not disliked by {account.Nickname} anymore");
            return LikeOperationResult.DislikeRemoved;
        }
        else
        {
            var LikeToRemove = _dbContext.Likes.SingleOrDefault(l => l.Liked == picture && l.Liker == account);
            if (LikeToRemove is not null) _dbContext.Likes.Remove(LikeToRemove);
            _dbContext.Dislikes.Add(
                new Dislike()
                {
                    DisLiked = picture,
                    DisLiker = account
                });
            _dbContext.SaveChanges();
            _logger.LogInformation($"Picture {picture.Name} has been disliked by {account.Nickname}");
            return LikeOperationResult.Disliked;
        }
        
    }
    
}