using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace PicturesAPI.Services;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountContextService _accountContextService;
    private readonly IAccountRepo _accountRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly IPictureRepo _pictureRepo;
    private readonly IAuthorizationService _authorizationService;

    public AccountService(
        IMapper mapper,
        ILogger<AccountService> logger,
        IAccountContextService accountContextService,
        IAccountRepo accountRepo,
        ILikeRepo likeRepo,
        IPictureRepo pictureRepo,
        IAuthorizationService authorizationService)
    {        
        _mapper = mapper;
        _logger = logger;
        _accountContextService = accountContextService;
        _accountRepo = accountRepo;
        _likeRepo = likeRepo;
        _pictureRepo = pictureRepo;
        _authorizationService = authorizationService;
    }
        
    public AccountDto GetById(Guid id)
    {
        var account = _accountRepo.GetAccountById(id);
        var result = _mapper.Map<AccountDto>(account);
        
        return result;
    }

    public PagedResult<AccountDto> GetAll(AccountQuery query)
    {
        var baseQuery = _accountRepo.GetAccounts()
            .Where(a => query.SearchPhrase == null || a.Nickname.ToLower().Contains(query.SearchPhrase.ToLower()))
            .OrderByDescending(a => _pictureRepo.GetPicturesByOwner(a).Count())
            .ThenByDescending(a => _pictureRepo.GetPicturesByOwner(a).Sum(picture => picture.Likes.Count))
            .ToList();

        var accounts = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();
        
        if (accounts.Count == 0) throw new NotFoundException("accounts not found");
        
        var resultCount = baseQuery.Count;

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts).ToList();
        var result = new PagedResult<AccountDto>(accountDtos, resultCount, query.PageSize, query.PageNumber);
        
        return result;
    }
    
    public IEnumerable<AccountDto> GetAllOdata()
    {
        var accounts = _accountRepo.GetAccounts().ToList();
            
        if (accounts.Count == 0) throw new NotFoundException("accounts not found");
            
        var result = _mapper.Map<List<AccountDto>>(accounts);
        return result;
    }

    public string GetLikedTags()
    {
        var id = _accountContextService.GetAccountId;
        var tags = _accountRepo.GetLikedTags(Guid.Parse(id));

        return tags;
    }
    
    public bool Update(PutAccountDto dto)
    {
        var user = _accountContextService.User;
        var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        var isUpdated = _accountRepo.UpdateAccount(dto, id);
        return isUpdated;
    }

    public bool Delete(Guid id)
    {
        var account = _accountRepo.GetAccountById(id);
        var user = _accountContextService.User;

        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE action invoked");
        var authorizationResult = _authorizationService.AuthorizeAsync(user, account, new AccountOperationRequirement(AccountOperation.Delete)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException("You have no rights to delete this account");
        
        var accountDeleteResult = _accountRepo.DeleteAccount(account);

        _logger.LogWarning(
            "Account with " +
            $"Nickname: {account.Nickname}, " +
            $"Id: {account.Id} " +
            $"DELETE action result: {accountDeleteResult}");
        
        return accountDeleteResult;
    }
    
    


}