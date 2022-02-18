#nullable enable
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class AccountRepo : IAccountRepo
{
    private readonly PictureDbContext _dbContext;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountRepo(
        PictureDbContext dbContext, 
        IPasswordHasher<Account> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public Account? GetAccountById(Guid id, DbInclude option)
    {
        Account? account;
        if (option == DbInclude.Include)
        {
            account = _dbContext.Accounts
                .Include(a => a.Pictures)
                .ThenInclude(p => p.Likes)
                .Include(a => a.Likes)
                .AsSplitQuery()
                .SingleOrDefault(a => a.Id == id);
        } else
        {
            account = _dbContext.Accounts
                .SingleOrDefault(a => a.Id == id);
        }

        return account;
    }

    public Account? GetAccountByNick(string nickname, DbInclude option)
    {
        Account? account;
        if (option == DbInclude.Include)
        {
            account = _dbContext.Accounts
                .Include(a => a.Pictures)
                .ThenInclude(p => p.Likes)
                .Include(a => a.Likes)
                .AsSplitQuery()
                .SingleOrDefault(a => a.Nickname == nickname);
        } else
        {
            account = _dbContext.Accounts
                .SingleOrDefault(a => a.Nickname == nickname);
        }
        
        return account;
    }
    
    public IEnumerable<Account>? GetAccounts(DbInclude option)
    {
        IEnumerable<Account>? accounts;

        if (option == DbInclude.Include)
        {
            accounts = _dbContext.Accounts
                .Include(p => p.Pictures)
                .ThenInclude(p => p.Likes)
                .Include(p => p.Likes);
        } else
        {
            accounts = _dbContext.Accounts;
        }

        return accounts;
    }
    
    public string GetLikedTags(Guid accId)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id == accId);
        return account!.LikedTags ?? string.Empty;
    }
    
    public Guid CreateAccount(Account newAccount)
    {
        _dbContext.Accounts.Add(newAccount);
        _dbContext.SaveChanges();
        
        return newAccount.Id;
    }

    //  is updating things like that in a repo a good practice?
    //  |
    //  V
    public void UpdateAccount(PutAccountDto dto, string id)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id.ToString() == id)!;
        if (dto.Password is not null)
        {
            var passwordHashed = _passwordHasher.HashPassword(account!, dto.Password);
            account!.PasswordHash = passwordHashed;
        }
        if (dto.Email is not null) account.Email = dto.Email;
    }

    public void DeleteAccount(Guid id)
    {
        var accountToRemove = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
        accountToRemove!.IsDeleted = true;
        _dbContext.SaveChanges();
    }
    
    public void AddLikedTags(Account acc, Picture picture)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a == acc);
        
        var tagsToAdd = picture.Tags.Split(' ').Take(3).ToList();
        
        var accountTags = account!.LikedTags is null 
            ? new List<string>() 
            : account.LikedTags.Split(' ').ToList();
        
        accountTags.AddRange(tagsToAdd);
        accountTags = accountTags.Distinct().ToList();

        if (accountTags.Count > 20) accountTags = accountTags.Take(20).ToList();
        
        account.LikedTags = string.Join(' ', accountTags);
        _dbContext.SaveChanges();
    }

    public void RemoveLikedTags(Account acc, Picture picture)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a == acc);

        var tagsToRemove = picture.Tags.Split(' ').Take(3).ToList();
        
        var accountTags = account!.LikedTags is null 
            ? new List<string>() 
            : account.LikedTags.Split(' ').ToList();

        foreach (var tag in tagsToRemove)
        {
            accountTags.Remove(tag);
        }
        accountTags = accountTags.Distinct().ToList();
        account.LikedTags = string.Join(' ', accountTags);
        _dbContext.SaveChanges();
    }

}