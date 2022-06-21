using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class LikeRepo : ILikeRepo
{
    private readonly PictureDbContext _dbContext;

    public LikeRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Like GetById(int id)
    {
        return _dbContext.Likes.SingleOrDefault(l => l.Id == id);
    }

    public List<Like> GetByLikerId(int id)
    {
        return _dbContext.Likes.Where(l => l.Liker.Id == id)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToList();
    }

    public List<Like> GetByLikedId(int id)
    {
        return _dbContext.Likes.Where(l => l.Liked.Id == id)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToList();
    }

    public Like GetByLikerIdAndLikedId(int accountId, int pictureId)
    {
        return _dbContext.Likes
            .Include(l => l.Liker)
            .Include(l => l.Liked)
            .FirstOrDefault(l => l.Liker.Id == accountId && l.Liked.Id == pictureId);
    }

    public void Insert(Like like)
    {
        _dbContext.Likes.Add(like);
    }

    public void DeleteById(int id)
    {
        var like = _dbContext.Likes.SingleOrDefaultAsync(l => l.Id == id).Result;
        _dbContext.Likes.Remove(like!);
    }

    public void Update(Like like)
    {
        _dbContext.Likes.Update(like);
    }

    public bool Save()
    {
        return _dbContext.SaveChanges() > 0;
    }
    
}