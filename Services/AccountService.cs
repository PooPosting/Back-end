using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;

namespace PicturesAPI.Services;

public class AccountService : IAccountService
{
    private readonly PictureDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IMapper mapper,
        PictureDbContext dbContext, 
        IPasswordHasher<Account> passwordHasher, 
        ILogger<AccountService> logger)
            
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
        
    public AccountDto GetAccountById(Guid id)
    {
        var account = _dbContext.Accounts
            .Include(a => a.Pictures)
            .Include(a => a.Likes)
            .Include(a => a.Dislikes)
            .SingleOrDefault(a => a.Id == id);

        if (account == null) throw new NotFoundException("account not found");
            
        var result = _mapper.Map<AccountDto>(account);
        return result;
    }

    public IEnumerable<AccountDto> GetAllAccounts()
    {
        var accounts = _dbContext.Accounts
            .Include(a => a.Pictures)
            .Include(a => a.Likes)
            .Include(a => a.Dislikes)
            .ToList();
            
        if (accounts.Count == 0) throw new NotFoundException("accounts not found");
            
        var result = _mapper.Map<List<AccountDto>>(accounts);
        return result;
    }
        
    public void UpdateAccount(Guid id, PutAccountDto dto)
    {
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
        if (account is null) throw new NotFoundException("There's not such an account with that ID");
            
        var passwordHashed = _passwordHasher.HashPassword(account, dto.Password);
            
        if (dto.Email != null) account!.Email = dto.Email;
        if (dto.Password != null) account!.PasswordHash = passwordHashed;
        _dbContext.SaveChanges();
    }

    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    public void DeleteAccount(Guid id)
    {
        _logger.LogWarning($"Account with id: {id} DELETE action invoked");
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
        if (account is null)
        {
            _logger.LogWarning($"Account with id: {id} DELETE action failed (not found)");
            throw new NotFoundException("There's not such an account with that ID");
        }
        var likes = _dbContext.Likes.Where(l => l.Liker == account);
        var disLikes = _dbContext.Dislikes.Where(d => d.DisLiker == account);
            
        _dbContext.Likes.RemoveRange(likes);
        _dbContext.Dislikes.RemoveRange(disLikes);
        _dbContext.Accounts.Remove(account);
        _dbContext.SaveChanges();
        _logger.LogWarning($"Account with id: {id} DELETE action success");
    }

}