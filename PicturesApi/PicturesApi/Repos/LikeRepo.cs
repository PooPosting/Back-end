#nullable enable
using System.Linq.Expressions;
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

    public async Task<Like?> GetByIdAsync(
        int id
        )
    {
        return await _dbContext.Likes.SingleOrDefaultAsync(l => l.Id == id);
    }

    public async Task<int> CountLikesAsync(
        Expression<Func<Like, bool>> predicate
        )
    {
        return await _dbContext.Likes
            .Where(predicate)
            .CountAsync();
    }

    public async Task<IEnumerable<Like>> GetByPictureIdAsync(
        int picId,
        int itemsToSkip,
        int itemsToTake
        )
    {
        return await _dbContext.Likes
            .Where(l => l.PictureId == picId)
            .OrderByDescending(l => l.PictureId)
            .Include(a => a.Account)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .ToArrayAsync();
    }

}