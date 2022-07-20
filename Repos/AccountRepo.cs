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

    public async Task<int> CountAccountsAsync(Expression<Func<Account, bool>> filterExp)
    {
        return await _dbContext.Accounts
            .Where(filterExp)
            .CountAsync();
    }

    public async Task<int> CountAccountsAsync()
    {
        return await _dbContext.Accounts
            .CountAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _dbContext.Accounts
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Comments)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Likes)
            .Include(a => a.Role)
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByIdAsync(int id,
        params Expression<Func<Account, object>>[] includes)
    {
        var query = _dbContext.Accounts.AsQueryable();

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                query.Include(include);
            }
        }

        return await query.SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByNickAsync(string nickname,
        params Expression<Func<Account, object>>[] includes)
    {
        var query = _dbContext.Accounts.AsQueryable();
        if (includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.SingleOrDefaultAsync(a => a.Nickname == nickname);
    }

    public async Task<IEnumerable<Account>> SearchAllAsync(
        int itemsToSkip, int itemsToTake, string? searchPhrase,
        params Expression<Func<Account, object>>[] includes)
    {
        var query = _dbContext.Accounts
            .AsNoTracking()
            .Where(a => string.IsNullOrEmpty(searchPhrase) || a.Nickname.ToLower().Contains(searchPhrase.ToLower()))
            .OrderByDescending(a => a.Pictures.Sum(picture => picture.Likes.Count))
            .ThenByDescending(a => a.Pictures.Count)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery();

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
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

}