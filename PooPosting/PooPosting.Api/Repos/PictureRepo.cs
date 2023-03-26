#nullable enable
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Entities.Joins;

namespace PooPosting.Api.Repos;

public class PictureRepo : IPictureRepo
{
    private readonly PictureDbContext _dbContext;

    public PictureRepo(
        PictureDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }

    public async Task<int> CountPicturesAsync(
        Expression<Func<Picture, bool>> predicate
        )
    {
        return await _dbContext.Pictures
            .Where(predicate)
            .CountAsync();
    }

    public async Task<int> CountPicturesAsync()
    {
        return await _dbContext.Pictures
            .CountAsync();
    }

    public async Task<Picture?> GetByIdAsync(
        int id
        )
    {
        return await _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Account)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Picture>> GetNotSeenByAccountIdAsync(
        int accountId,
        int itemsToTake
        )
    {
        return await _dbContext.Pictures

            .OrderByDescending(p => (p.PictureTags
                .Select(t => t.Tag)
                .SelectMany(t => t.AccountLikedTags)
                .Select(alt => alt.AccountId == accountId).Count() + 1) * p.PopularityScore)
            .ThenByDescending(p => p.Id)
            .Where(p => !_dbContext.PicturesSeenByAccounts
                .Where(j => j.Account.Id == accountId)
                .Any(j => j.Picture.Id == p.Id && j.Account.Id == accountId))

            .Include(p => p.Account)
            .Include(p => p.Comments)
            .Include(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .AsNoTracking()

            .Take(itemsToTake)
            .ToListAsync();
    }

    public async Task<IEnumerable<Picture>> GetLikedPicturesByAccountIdAsync(
        int accountId,
        int itemsToSkip,
        int itemsToTake
    )
    {
        return await _dbContext.Pictures
            .AsNoTracking()
            .Where(p => p.Likes.Any(l => ((l.AccountId == accountId) && l.IsLike)))
            .OrderByDescending(p => p.Likes.Max(l => l.Id))
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .ToListAsync();
    }

    public async Task<IEnumerable<Picture>> GetPostedPicturesByAccountIdAsync(
        int accountId,
        int itemsToSkip,
        int itemsToTake
        )
    {
        return await _dbContext.Pictures
            .AsNoTracking()
            .Where(p => p.AccountId == accountId)
            .OrderByDescending(p => p.Id)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .ToListAsync();
    }

    public async Task<IEnumerable<Picture>> SearchAllAsync(
        int itemsToSkip, int itemsToTake,
        Expression<Func<Picture, long>>? orderExp,
        Expression<Func<Picture, bool>>? filterExp
        )
    {
        var query = _dbContext.Pictures
            .Include(p => p.Comments)
            .Include(p => p.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .AsNoTracking()
            .AsQueryable();

        if (orderExp is not null)
        {
            query = query.OrderByDescending(orderExp)
                .ThenByDescending(p => p.Id);
        }

        if (filterExp is not null)
        {
            query = query.Where(filterExp);
        }

        return await query
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .ToListAsync();
    }

    public async Task<Picture> InsertAsync(
        Picture picture
        )
    {
        await _dbContext.Pictures.AddAsync(picture);

        await _dbContext.SaveChangesAsync();
        return picture;
    }

    public async Task<Picture> UpdateAsync(
        Picture picture
        )
    {
        _dbContext.Pictures.Update(picture);
        await _dbContext.SaveChangesAsync();
        return picture;
    }
}