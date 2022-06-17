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

    public async Task<List<Like>> GetByLikerId(Guid id)
    {
        return await _dbContext.Likes.Where(l => l.Liker.Id == id)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToListAsync();
    }

    public async Task<List<Like>> GetByLikedId(Guid id)
    {
        return await _dbContext.Likes.Where(l => l.Liked.Id == id)
            .Include(a => a.Liked)
            .Include(a => a.Liker)
            .ToListAsync();
    }

    public async Task<Like> GetByLikerIdAndLikedId(Guid accountId, Guid pictureId)
    {
        return await _dbContext.Likes
            .Include(l => l.Liker)
            .Include(l => l.Liked)
            .FirstOrDefaultAsync(l => l.Liker.Id == accountId && l.Liked.Id == pictureId);
    }

    public async Task Insert(Like like)
    {
        await _dbContext.Likes.AddAsync(like);
    }

    public async Task Delete(Like like)
    {
        _dbContext.Remove(like);
    }

    public async Task Update(Like like)
    {
        _dbContext.Likes.Update(like);
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
    
}