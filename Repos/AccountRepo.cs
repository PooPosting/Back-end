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
        switch (option) 
        {
            case DbInclude.Include:
            {
                account = _dbContext.Accounts
                    .Include(a => a.Pictures)
                    .ThenInclude(p => p.Likes)
                    .ThenInclude(p => p.Liker)
                    .Include(a => a.Pictures)
                    .ThenInclude(p => p.Comments)
                    .ThenInclude(c => c.Author)
                    .Include(a => a.Likes)
                    .AsSplitQuery()
                    .SingleOrDefault(a => a.Id == id);
                break;
            }
            case DbInclude.Raw:
            {
                account = _dbContext.Accounts
                    .SingleOrDefault(a => a.Id == id);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(option), option, null);
        }
        return account;
    }

    public Account? GetAccountByNick(string nickname, DbInclude option)
    {
        Account? account;
        switch (option) 
        {
            case DbInclude.Include:
            {
                account = _dbContext.Accounts
                    .Include(a => a.Pictures)
                    .ThenInclude(p => p.Likes)
                    .ThenInclude(p => p.Liker)
                    .Include(a => a.Pictures)
                    .ThenInclude(p => p.Comments)
                    .ThenInclude(c => c.Author)
                    .Include(a => a.Likes)
                    .AsSplitQuery()
                    .SingleOrDefault(a => a.Nickname == nickname);
                break;
            }
            case DbInclude.Raw:
            {
                account = _dbContext.Accounts
                    .SingleOrDefault(a => a.Nickname == nickname);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(option), option, null);
        }
        return account;
    }

    public IEnumerable<Account>? GetAccounts(DbInclude option)
    {
        IEnumerable<Account>? accounts;
        switch (option)
        {
            case DbInclude.Include:
            {
                accounts = _dbContext.Accounts
                    .Include(p => p.Pictures)
                    .ThenInclude(p => p.Likes)
                    .Include(p => p.Likes)
                    .AsSplitQuery();
                break;
            }
            case DbInclude.Raw:
            {
                accounts = _dbContext.Accounts;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(option), option, null);
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
    
    public void UpdateAccount(PutAccountDto dto, Guid id)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id == id)!;
        if (dto.Email is not null)
        {
            account.Email = dto.Email;
        }
        if (dto.Password is not null)
        {
            var passwordHashed = _passwordHasher.HashPassword(account, dto.Password);
            account!.PasswordHash = passwordHashed;
        }
        _dbContext.SaveChanges();
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

        if (accountTags.Count > 50) accountTags = accountTags.TakeLast(50).ToList();
        
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

    // public string SetVerificationCode(Guid id)
    // {
    //     var rnd = new Random();
    //     var confToken = rnd.Next(100000, 999999).ToString();
    //     var acc = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
    //     acc!.ConfirmationToken = confToken;
    //     return confToken;
    // }
    public bool Exists(Guid id)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
        if (account is not null) return !account.IsDeleted;
        return false;
    }

}