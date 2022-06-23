using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class PictureRepo : IPictureRepo
{
    private readonly PictureDbContext _dbContext;

    public PictureRepo(
        PictureDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }

    public Picture GetById(int id)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .ThenInclude(c => c.Account)
            .Include(p => p.Account)
            .AsSplitQuery()
            .SingleOrDefault(p => p.Id == id);
    }

    public IEnumerable<Picture> GetFromAll(int itemsToSkip, int itemsToTake)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Include(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(p => p.Account)
            .OrderByDescending(p => p.PopularityScore)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery()
            .ToList();
    }

    public IEnumerable<Picture> GetNotSeenByAccountId(int accountId, int itemsToTake)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Where(p => !_dbContext.PictureSeenByAccountJoins
                .Where(j => j.AccountId == accountId)
                .AsSplitQuery()
                .Any(j => j.PictureId == p.Id && j.AccountId == accountId))
            .Include(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .OrderByDescending(p => p.PopularityScore *
                                    (p.PictureTagJoins.Select(j =>
                                         j.Tag.AccountLikedTagJoins.Select(a => a.AccountId == accountId)).Count() *
                                     2.5)
            )
            .Take(itemsToTake)
            .AsSplitQuery();
    }

    public IEnumerable<Picture> SearchAll(int itemsToSkip, int itemsToTake, string searchPhrase)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Where(p => searchPhrase == null || p.Name.ToLower().Contains(searchPhrase.ToLower()))
            .Include(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(p => p.Account)
            .OrderByDescending(p => p.PopularityScore)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery();
    }

    public IEnumerable<Picture> SearchNewest(int itemsToSkip, int itemsToTake, string searchPhrase)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Where(p => searchPhrase == null || p.Name.ToLower().Contains(searchPhrase.ToLower()))
            .Include(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(p => p.Account)
            .OrderByDescending(p => p.PictureAdded)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery();
    }

    public IEnumerable<Picture> SearchMostLikes(int itemsToSkip, int itemsToTake, string searchPhrase)
    {
        return _dbContext.Pictures
            .Where(p => !p.IsDeleted)
            .Where(p => searchPhrase == null || p.Name.ToLower().Contains(searchPhrase.ToLower()))
            .Include(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .Include(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(p => p.Account)
            .OrderByDescending(p => p.Likes.Count(l => l.IsLike))
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery();
    }

    public int Insert(Picture picture)
    {
        _dbContext.Pictures.Add(picture);
        return picture.Id;
    }

    public void Update(Picture picture)
    {
        _dbContext.Pictures.Update(picture);
    }

    public void DeleteById(int id)
    {
        var pic = _dbContext.Pictures.SingleOrDefault(p => p.Id == id);
        pic.IsDeleted = true;
    }
    
    public bool Save()
    {
        return _dbContext.SaveChanges() > 0;
    }
}