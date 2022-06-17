#nullable enable
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class AccountRepo : IAccountRepo
{
    private readonly PictureDbContext _dbContext;

    public AccountRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Account>> GetAll()
    {
        return await _dbContext.Accounts
            .Include(p => p.Pictures)
            .ThenInclude(p => p.Likes)
            .Include(p => p.Likes)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Account> GetById(Guid id)
    {
        return (await _dbContext.Accounts
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Likes)
            .ThenInclude(p => p.Liker)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Comments)
            .ThenInclude(c => c.Author)
            .Include(a => a.Likes)
            .AsSplitQuery()
            .SingleOrDefaultAsync(a => a.Id == id))!;
    }

    public async Task<Guid> Insert(Account newAccount)
    {
        await _dbContext.Accounts.AddAsync(newAccount);
        return newAccount.Id;
    }

    public async Task Update(Account account)
    {
        _dbContext.Accounts.Update(account);
    }

    public async Task Delete(Guid id)
    {
        var accountToRemove = await _dbContext.Accounts.SingleOrDefaultAsync(a => a.Id == id);
        accountToRemove!.IsDeleted = true;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    // public async Task AddLikedTags(Account acc, Picture picture)
    // {
    //     var tagsToAdd = picture.Tags.Split(' ').Take(3).ToList();
    //
    //     var accountTags = acc!.LikedTags is null
    //         ? new List<string>()
    //         : acc.LikedTags.Split(' ').ToList();
    //
    //     accountTags.AddRange(tagsToAdd);
    //     accountTags = accountTags.Distinct().ToList();
    //     if (accountTags.Count > 50) accountTags = accountTags.TakeLast(50).ToList();
    //     acc.LikedTags = string.Join(' ', accountTags);
    //
    //     ctx.Accounts.Update(acc);
    //     await ctx.SaveChangesAsync();
    // }
    //
    // public async Task RemoveLikedTags(Account acc, Picture picture)
    // {
    //     var tagsToRemove = picture.Tags.Split(' ').Take(3).ToList();
    //     var accountTags = acc!.LikedTags is null
    //         ? new List<string>()
    //         : acc.LikedTags.Split(' ').ToList();
    //     foreach (var tag in tagsToRemove)
    //     {
    //         accountTags.Remove(tag);
    //     }
    //     accountTags = accountTags.Distinct().ToList();
    //     acc.LikedTags = string.Join(' ', accountTags);
    //
    //     ctx.Accounts.Update(acc);
    //     await ctx.SaveChangesAsync();
    // }


}