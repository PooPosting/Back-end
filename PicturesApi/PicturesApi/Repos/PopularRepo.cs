#nullable enable
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class PopularRepo : IPopularRepo
{
    private readonly PictureDbContext _dbContext;

    public PopularRepo(
        PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Picture>> GetPicsByVoteCountAsync(int itemsToTake)
    {
        return await _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.Likes.Count())
            .Take(itemsToTake)
            .Include(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(p => p.Account)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IEnumerable<Picture>> GetPicsByLikeCountAsync(int itemsToTake)
    {
        return await _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.Likes.Count(l => l.IsLike))
            .Take(itemsToTake)
            .Include(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(p => p.Account)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IEnumerable<Picture>> GetPicsByCommentCountAsync(int itemsToTake)
    {
        return await _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.Comments.Count())
            .Take(itemsToTake)
            .Include(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(p => p.Account)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IEnumerable<Account>> GetAccsByPostCountAsync(int itemsToTake)
    {
        return await _dbContext.Accounts
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.Pictures.Count())
            .Take(itemsToTake)
            .Include(a => a.Likes)
            .Include(a => a.Comments
                .Where(c => c.IsDeleted == false))
            .Include(a => a.Pictures)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IEnumerable<Account>> GetAccsByPostLikesCountAsync(int itemsToTake)
    {
        return await _dbContext.Accounts
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.Pictures.Sum(p => p.Likes.Count()))
            .Take(itemsToTake)
            .Include(a => a.Likes)
            .Include(a => a.Comments
                .Where(c => c.IsDeleted == false))
            .Include(a => a.Pictures)
            .AsSplitQuery()
            .ToListAsync();
    }
}