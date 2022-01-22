using System.Collections.Generic;
using System.Linq;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class LikeRepo : ILikeRepo
{
    private readonly PictureDbContext _dbContext;

    public LikeRepo(
        PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Like> GetLikesByLiker(Account liker)
    {
        var likes = _dbContext.Likes.Where(l => l.Liker == liker).ToList();
        return likes;
    }

    public List<Like> GetLikesByLiked(Picture picture)
    {
        var likes = _dbContext.Likes.Where(l => l.Liked == picture).ToList();
        return likes;
    }

    public Like GetLikeByLikerAndLiked(Account account, Picture picture)
    {
        var like = _dbContext.Likes
            .FirstOrDefault(l => l.Liker == account && l.Liked == picture);
        return like;
    }

    public bool AddLike(Like like)
    {
        _dbContext.Add(like);
        _dbContext.SaveChanges();
        return true;
    }

    public bool RemoveLike(Like like)
    {
        if (_dbContext.Likes.Any(l => l == like))
        {
            _dbContext.Remove(like);
            _dbContext.SaveChanges();
            return true;
        }
        return false;
    }

    public bool ChangeLike(Like like)
    {
        var likeToChange = _dbContext.Likes.SingleOrDefault(l => l == like);
        
        if (likeToChange is not null)
        {
            likeToChange.IsLike = !likeToChange.IsLike;
            _dbContext.SaveChanges();
            return true;
        }

        return false;
    }
    
    public int RemoveLikes(List<Like> likes)
    {
        _dbContext.Likes.RemoveRange(likes);
        _dbContext.SaveChanges();
        
        return likes.Count;
    }
    
    
    
}