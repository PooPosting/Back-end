using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Enums;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Mappers;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly PictureDbContext _dbContext;
    private readonly IAccountContextService _accountContextService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountService(
        ILogger<AccountService> logger,
        PictureDbContext dbContext,
        IAccountContextService accountContextService,
        IAuthorizationService authorizationService,
        IPasswordHasher<Account> passwordHasher)
    {        
        _logger = logger;
        _dbContext = dbContext;
        _accountContextService = accountContextService;
        _authorizationService = authorizationService;
        _passwordHasher = passwordHasher;
    }
        
    public async Task<AccountDto> GetById(int id)
    {
        var currAccId = _accountContextService.TryGetAccountId();
        
        var account = await _dbContext.Accounts
            .Where(a => a.Id == id)
            .ProjectToDto(currAccId)
            .FirstOrDefaultAsync();

        return account ?? throw new NotFoundException();
    }

    public async Task<PagedResult<AccountDto>> GetAll(SearchQuery query)
    {
        var accountsQueryable= _dbContext.Accounts
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize);

        if (query.SearchPhrase is not null)
        {
            accountsQueryable = accountsQueryable
                .Where(a =>
                    a.Nickname.ToLower().Contains(query.SearchPhrase.ToLower())
                );
        }

        var count = await accountsQueryable.CountAsync();
        
        var currAccId = _accountContextService.TryGetAccountId();
        var accountDtos = await accountsQueryable
            .ProjectToDto(currAccId)
            .ToListAsync();

        var result = new PagedResult<AccountDto>(
            accountDtos,
            query.PageNumber,
            query.PageSize,
            count
        );
        return result;
    }

    public async Task<AccountDto> UpdateEmail(UpdateAccountEmailDto dto)
    {
        var account = await _accountContextService.GetAccountAsync();
        if (account == null) throw new NotFoundException();
        account.Email = dto.Email;
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
        return account.MapToDto(account.Id);
    }
    
    public async Task<AccountDto> UpdateDescription(UpdateAccountDescriptionDto dto)
    {
        var account = await _accountContextService.GetAccountAsync();
        var result = _dbContext.Update(account).Entity.MapToDto(account.Id);
        await _dbContext.SaveChangesAsync();
        return result;
    }
    
    public async Task<AccountDto> UpdatePassword(UpdateAccountPasswordDto dto)
    {
        var account = await _accountContextService.GetAccountAsync();
        account.PasswordHash = _passwordHasher.HashPassword(account, dto.Password);
        var result = _dbContext.Update(account).Entity.MapToDto(account.Id);
        await _dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<AccountDto> UpdateBackgroundPicture(UpdateAccountPictureDto dto)
    {
        var account = await _accountContextService.GetAccountAsync();
        if (!dto.File.ContentType.StartsWith("image")) throw new BadRequestException("invalid picture");
        var fileExt = dto.File.ContentType.EndsWith("gif") ? "gif" : "webp";
        var bgName = $"{IdHasher.EncodeAccountId(account.Id)}-{DateTime.Now.ToFileTimeUtc()}-bgp.{fileExt}";
        var result = _dbContext.Update(account).Entity.MapToDto(account.Id);
        await _dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<AccountDto> UpdateProfilePicture(UpdateAccountPictureDto dto)
    {
        var account = await _accountContextService.GetAccountAsync();
        if (!dto.File.ContentType.StartsWith("image")) throw new BadRequestException("invalid picture");
        var fileExt = dto.File.ContentType.EndsWith("gif") ? "gif" : "webp";
        var bgName = $"{IdHasher.EncodeAccountId(account.Id)}-{DateTime.Now.ToFileTimeUtc()}-bgp.{fileExt}";
        account.ProfilePicUrl = Path.Combine("wwwroot", "accounts", "profile_pictures", $"{bgName}");
        var result = _dbContext.Update(account).Entity.MapToDto(account.Id);
        await _dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<bool> Delete(int id)
    {
        var account = await _accountContextService.GetTrackedAccountAsync();
        _logger.LogWarning("Account with Nickname: {AccountNickname} DELETE (SOFT) action invoked", account.Nickname);
        await AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account");

        account.IsDeleted = true;
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
        _logger.LogWarning("Account with Nickname: {AccountNickname}, Id: {AccountId} DELETE (SOFT) action succeed", account.Nickname, account.Id);
        return account.IsDeleted;
    }

    #region Private methods

    private async Task AuthorizeAccountOperation(Account account, ResourceOperation operation, string message)
    {
        var user = _accountContextService.User;
        var authorizationResult = await _authorizationService.AuthorizeAsync(user, account, new AccountOperationRequirement(operation));
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

    #endregion

}