using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services;

public class AccountService : IAccountService
{
    private readonly PictureDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountContextService _accountContextService;

    // DONT USE DBCONTEXT IN ANY SERVICE
    // USE FACTORY
    // DUMBASS

    public AccountService(
        IMapper mapper,
        PictureDbContext dbContext, 
        IPasswordHasher<Account> passwordHasher, 
        ILogger<AccountService> logger,
        IAccountContextService accountContextService)
            
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _accountContextService = accountContextService;
    }
        
    public AccountDto GetById(Guid id)
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

    public PagedResult<AccountDto> GetAll(AccountQuery query)
    {
        var baseQuery = _dbContext.Accounts
            .Include(p => p.Pictures)
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .Where(p => query.SearchPhrase == null || p.Nickname.ToLower().Contains(query.SearchPhrase.ToLower()));
        
        var accounts = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        
        if (accounts.Count == 0) throw new NotFoundException("pictures not found");
        
        var resultCount = baseQuery.Count();

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts).ToList();
        var result = new PagedResult<AccountDto>(accountDtos, resultCount, query.PageSize, query.PageNumber);
        
        return result;
    }

    public IEnumerable<AccountDto> GetAllOdata()
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
        
    public void Update(PutAccountDto dto)
    {
        var user = _accountContextService.User;
        var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id.ToString() == id);
        if (account is null) throw new NotFoundException("There's not such an account with that ID");
            
        var passwordHashed = _passwordHasher.HashPassword(account, dto.Password);
            
        if (dto.Email != null) account!.Email = dto.Email;
        if (dto.Password != null) account!.PasswordHash = passwordHashed;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        _logger.LogWarning($"Account with id: {id} DELETE action invoked");
        var account = _dbContext.Accounts.SingleOrDefault(a => a.Id == id);
        if (account is null)
        {
            _logger.LogWarning($"Account with id: {id} DELETE action failed (not found)");
            throw new NotFoundException("There's not such an account with that ID");
        }
        var likes = _dbContext.Likes.Where(l => l.Liker == account);
            
        _dbContext.Likes.RemoveRange(likes);
        _dbContext.Accounts.Remove(account);
        _dbContext.SaveChanges();
        _logger.LogWarning($"Account with id: {id} DELETE action success");
    }

}