#nullable enable
using System.Linq.Expressions;
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

    public async Task<int> CountAccountsAsync(Expression<Func<Account, bool>> predicate)
    {
        return await _dbContext.Accounts
            .Where(a => !a.IsDeleted)
            .Where(predicate)
            .CountAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _dbContext.Accounts
            .Include(a => a.Pictures)
            .ThenInclude(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
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
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByNickAsync(string nickname)
    {
        return await _dbContext.Accounts
            .Include(a => a.Pictures)
            .ThenInclude(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Likes)
            .ThenInclude(p => p.Liker)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Comments)
            .ThenInclude(c => c.Account)
            .Include(a => a.Likes)
            .Include(a => a.Role)
            .AsSplitQuery()
            .SingleOrDefaultAsync(a => a.Nickname == nickname);
    }

    public async Task<IEnumerable<Account>> SearchAllAsync(int itemsToSkip, int itemsToTake, string searchPhrase)
    {
        return await _dbContext.Accounts
            .Where(a => !a.IsDeleted)
            .Where(a => searchPhrase == string.Empty || a.Nickname.ToLower().Contains(searchPhrase.ToLower()))
            .OrderByDescending(a => a.Pictures.Sum(picture => picture.Likes.Count))
            .ThenByDescending(a => a.Pictures.Count)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.PictureTagJoins)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Pictures)
            .ThenInclude(p => p.Likes)
            .Include(p => p.Pictures)
            .ThenInclude(p => p.Comments
                .Where(c => c.IsDeleted == false))
            .Include(a => a.Role)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Account> InsertAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
        return account;
    }

    public async Task<Account> UpdateAsync(Account account)
    {
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
        return account;
    }

    public async Task<bool> TryDeleteByIdAsync(int id)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
        if (account is not null)
        {
            account!.IsDeleted = true;
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<bool> MarkAsSeenAsync(int accountId, int pictureId)
    {
        if (!_dbContext.PictureSeenByAccountJoins
            .Any(j => (j.Account.Id == accountId) && (j.Picture.Id == pictureId)))
        {
            _dbContext.PictureSeenByAccountJoins.Add(new PictureSeenByAccountJoin()
            {
                Account = _dbContext.Accounts.SingleOrDefault(a => a.Id == accountId),
                Picture = _dbContext.Pictures.SingleOrDefault(p => p.Id == pictureId)
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }

}