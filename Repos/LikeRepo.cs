using Microsoft.EntityFrameworkCore;
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
        var likes = _dbContext.Likes.Where(l => l.Liker == liker)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToList();
        
        return likes;
    }
    public List<Like> GetLikesByLiker(Guid id)
    {
        var likes = _dbContext.Likes.Where(l => l.Liker.Id == id)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToList();
        
        return likes;
    }

    public List<Like> GetLikesByLiked(Picture picture)
    {
        var likes = _dbContext.Likes.Where(l => l.Liked == picture)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToList();
        return likes;
    }
    
    public List<Like> GetLikesByLiked(Guid id)
    {
        var likes = _dbContext.Likes.Where(l => l.Liked.Id == id)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToList();
        return likes;
    }

    public Like GetLikeByLikerAndLiked(Account account, Picture picture)
    {
        var like = _dbContext.Likes
            .FirstOrDefault(l => l.Liker == account && l.Liked == picture);
        return like;
    }

    public void AddLike(Like like)
    {
        _dbContext.Add(like);
        _dbContext.SaveChanges();
    }

    public void RemoveLike(Like like)
    {
        _dbContext.Remove(like);
        _dbContext.SaveChanges();
    }

    public void ChangeLike(Like like)
    {
        var likeToChange = _dbContext.Likes.SingleOrDefault(l => l == like);
        
        likeToChange!.IsLike = !likeToChange.IsLike;
        _dbContext.SaveChanges();
    }
    
    public int RemoveLikes(List<Like> likes)
    {
        _dbContext.Likes.RemoveRange(likes);
        _dbContext.SaveChanges();
        
        return likes.Count;
    }
    
    public bool Exists(int id)
    {
        var like = _dbContext.Likes.SingleOrDefault(l => l.Id == id);
        return (like is not null);
    }
    
}