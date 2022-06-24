#nullable enable
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class AccountRepo : IAccountRepo
{
    private readonly PictureDbContext _dbContext;

    public AccountRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Account> SearchAll(int itemsToSkip, int itemsToTake, string searchPhrase)
    {
        return _dbContext.Accounts
            .Where(a => !a.IsDeleted)
            .Where(a => searchPhrase == null || a.Nickname.ToLower().Contains(searchPhrase.ToLower()))
            .OrderByDescending(a => a.Pictures.Sum(picture => picture.Likes.Count))
            .ThenByDescending(a => a.Pictures.Count)
            .Include(p => p.Pictures)
            .ThenInclude(p => p.Likes)
            .Include(p => p.Pictures)
            .ThenInclude(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(a => a.Role)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery();
    }

    public Account GetById(int id)
    {
        return _dbContext.Accounts
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Likes)
            .ThenInclude(p => p.Liker)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .ThenInclude(c => c.Account)
            .Include(a => a.Likes)
            .Include(a => a.Role)
            .AsSplitQuery()
            .SingleOrDefault(a => a.Id == id)!;
    }

    public Account GetByNick(string nickname)
    {
        return _dbContext.Accounts
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Likes)
            .ThenInclude(p => p.Liker)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Comments)
            .ThenInclude(c => c.Account)
            .Include(a => a.Likes)
            .Include(a => a.Role)
            .AsSplitQuery()
            .SingleOrDefault(a => a.Nickname == nickname)!;
    }

    public void Insert(Account newAccount)
    {
        _dbContext.Accounts.Add(newAccount);
    }

    public void Update(Account account)
    {
        _dbContext.Accounts.Update(account);
    }

    public void DeleteById(int id)
    {
        var accountToRemove = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
        accountToRemove!.IsDeleted = true;
    }

    public void MarkAsSeen(int accountId, int pictureId)
    {
        if (!_dbContext.PictureSeenByAccountJoins
            .Any(j => (j.Account.Id == accountId) && (j.Picture.Id == pictureId)))
        {
            _dbContext.PictureSeenByAccountJoins.Add(new PictureSeenByAccountJoin()
            {
                Account = _dbContext.Accounts.SingleOrDefault(a => a.Id == accountId),
                Picture = _dbContext.Pictures.SingleOrDefault(p => p.Id == pictureId)
            });
        }
    }

    public bool Save()
    {
        return _dbContext.SaveChanges() > 0;
    }
}