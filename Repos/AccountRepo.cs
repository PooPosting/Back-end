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

    public List<Account> GetAll()
    {
        return _dbContext.Accounts
            .Include(p => p.Pictures)
            .ThenInclude(p => p.Likes)
            .Include(p => p.Likes)
            .Include(a => a.Role)
            .AsSplitQuery()
            .ToList();
    }

    public Account GetById(int id)
    {
        return _dbContext.Accounts
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Likes)
            .ThenInclude(p => p.Liker)
            .Include(a => a.Pictures)
            .ThenInclude(p => p.Comments)
            .ThenInclude(c => c.Author)
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
            .ThenInclude(c => c.Author)
            .Include(a => a.Likes)
            .Include(a => a.Role)
            .AsSplitQuery()
            .SingleOrDefault(a => a.Nickname == nickname)!;
    }

    public int Insert(Account newAccount)
    {
        _dbContext.Accounts.Add(newAccount);
        return newAccount.Id;
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

    public bool Save()
    {
        return _dbContext.SaveChanges() > 0;
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