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
            .Select(a => new Account()
            {
                Id = a.Id,
                Nickname = a.Nickname,
                Email = a.Email,
                Verified = a.Verified,
                ProfilePicUrl = a.ProfilePicUrl,
                BackgroundPicUrl = a.BackgroundPicUrl,
                AccountDescription = a.AccountDescription,
                AccountCreated = a.AccountCreated,
                Role = a.Role,
                Pictures = a.Pictures.Select(p => new Picture()
                {
                    Id = p.Id,
                    AccountId = p.AccountId,
                    Name = p.Name,
                    Url = p.Url,
                    PictureAdded = p.PictureAdded,
                    Comments = p.Comments.Select(c => new Comment()
                    {
                        Id = c.Id
                    }).AsEnumerable(),
                    Likes = p.Likes.Select(l => new Like()
                    {
                        Id = l.Id,
                    }).AsEnumerable(),
                }).OrderByDescending(p => p.PictureAdded).AsEnumerable(),
            })
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByNickAsync(string nickname)
    {
        return await _dbContext.Accounts
            .Include(a => a.Role)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Likes)
            .ThenInclude(p => p.Account)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Comments)
            .ThenInclude(c => c.Account)
            .Include(a => a.Likes)
            .Include(a => a.Role)
            .AsSplitQuery()
            .SingleOrDefaultAsync(a => a.Nickname == nickname);
    }

    public async Task<IEnumerable<Account>> SearchAllAsync(int itemsToSkip, int itemsToTake, string? searchPhrase)
    {
        var result = await _dbContext.Accounts
            .Where(a => !a.IsDeleted)
            .Where(a => string.IsNullOrEmpty(searchPhrase) || a.Nickname.ToLower().Contains(searchPhrase.ToLower()))
            .OrderByDescending(a => a.Pictures.Sum(picture => picture.Likes.Count()))
            .ThenByDescending(a => a.Pictures.Count())
            .Include(a => a.Role)
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .AsSplitQuery()
            .ToArrayAsync();
        return result;

        // return await _dbContext.Accounts
        //     .Where(a => string.IsNullOrEmpty(searchPhrase) || a.Nickname.ToLower().Contains(searchPhrase.ToLower()))
        //     .OrderByDescending(a => a.Pictures.Sum(picture => picture.Likes.Count))
        //     .ThenByDescending(a => a.Pictures.Count)
        //     .Select(a => new Account()
        //     {
        //         Pictures = a.Pictures,
        //
        //
        //     })
        //     .AsSplitQuery()
        //     .ToArrayAsync();
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
        if (!_dbContext.PicturesSeenByAccounts
            .Any(j => (j.Account.Id == accountId) && (j.Picture.Id == pictureId)))
        {
            _dbContext.PicturesSeenByAccounts.Add(new PictureSeenByAccount()
            {
                AccountId = accountId,
                PictureId = pictureId
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }

}