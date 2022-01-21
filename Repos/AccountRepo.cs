using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class AccountRepo : IAccountRepo
{
    private readonly PictureDbContext _dbContext;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IPictureRepo _pictureRepo;

    public AccountRepo(
        PictureDbContext dbContext, 
        IPasswordHasher<Account> passwordHasher,
        IPictureRepo pictureRepo)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _pictureRepo = pictureRepo;
    }

    public Account GetAccountById(Guid id)
    {
        var account = _dbContext.Accounts
            .Include(a => a.Pictures)
            .Include(a => a.Likes)
            .SingleOrDefault(a => a.Id == id);

        if (account == null) throw new NotFoundException("account not found");
        
        return account;
    }

    public Account GetAccountByNick(string nickname)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Nickname == nickname);
        return account;
    }
    
    public IEnumerable<Account> GetAccounts()
    {
        var accounts = _dbContext.Accounts
            .Include(p => p.Pictures)
            .Include(p => p.Likes);
        
        if (accounts.ToList().Count == 0) throw new NotFoundException("account not found");
        
        return accounts;
    }
    
    public Guid CreateAccount(CreateAccountDto dto)
    {
        var newAccount = new Account()
        {
            Nickname = dto.Nickname,
            Email = dto.Email,
            AccountCreated = DateTime.Now,
            Pictures = new List<Picture>(),
            RoleId = dto.RoleId
        };

        var hashedPassword = _passwordHasher.HashPassword(newAccount, dto.Password);
        newAccount.PasswordHash = hashedPassword;
        _dbContext.Accounts.Add(newAccount);
        _dbContext.SaveChanges();
        
        return newAccount.Id;
    }

    public bool UpdateAccount(PutAccountDto dto, string id)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id.ToString() == id);
        if (account is null) throw new InvalidAuthTokenException();
        
        if (dto.Password != null)
        {
            var passwordHashed = _passwordHasher.HashPassword(account, dto.Password);
            account!.PasswordHash = passwordHashed;
        }
        
        if (dto.Email != null) account!.Email = dto.Email;
        
        return true;
    }

    public bool DeleteAccount(Account account)
    {
        var accountToRemove = _dbContext.Accounts.SingleOrDefault(a => a == account);
        if (accountToRemove is null) throw new NotFoundException("Account not found");

        var picturesToRemove = _pictureRepo.GetPicturesByOwner(account).ToList();
        
        if(picturesToRemove.ToList().Count != 0) 
            _dbContext.Pictures.RemoveRange(picturesToRemove);
        
        _dbContext.Accounts.Remove(accountToRemove);
        _dbContext.SaveChanges();
        return true;
    }

}