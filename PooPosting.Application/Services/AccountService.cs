using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PooPosting.Api.Authorization;
using PooPosting.Application.Mappers;
using PooPosting.Application.Models;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Models.Queries;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Domain.DbContext;
using PooPosting.Domain.DbContext.Entities;
using PooPosting.Domain.Enums;
using PooPosting.Domain.Exceptions;

namespace PooPosting.Application.Services;

public class AccountService(ILogger<AccountService> logger,
        PictureDbContext dbContext,
        IAccountContextService accountContextService,
        IAuthorizationService authorizationService,
        IPasswordHasher<Account> passwordHasher)
    : IAccountService
{
    public async Task<AccountDto> GetById(int id)
    {
        var currAccId = accountContextService.TryGetAccountId();
        
        var account = await dbContext.Accounts
            .Where(a => a.Id == id)
            .ProjectToDto(currAccId)
            .FirstOrDefaultAsync();

        return account ?? throw new NotFoundException();
    }

    public async Task<AccountDto> GetCurrent()
    {
        var accId = accountContextService.TryGetAccountId();
        if (accId is null) throw new UnauthorizedException();
        
        var acc = await dbContext.Accounts
            .Where(a => a.Id == accId)
            .ProjectToDto(accId    )
            .FirstOrDefaultAsync();
        
        return acc!;
    }

    public async Task<PagedResult<AccountDto>> GetAll(AccountSearchQuery query)
    {
        var accountsQueryable = dbContext.Accounts
            .OrderBy(a => a.AccountCreated)
            .AsQueryable();

        if (query.SearchPhrase is not null)
        {
            accountsQueryable = accountsQueryable
                .Where(a => a.Nickname.Contains(query.SearchPhrase));
        }

        var count = await accountsQueryable.CountAsync();
        
        var currAccId = accountContextService.TryGetAccountId();

        var result = new PagedResult<AccountDto>(
            await accountsQueryable
                .ProjectToDto(currAccId)
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync(),
            query.PageNumber,
            query.PageSize,
            count
        );
        return result;
    }

    public async Task<AccountDto> UpdateEmail(UpdateAccountEmailDto dto)
    {
        var account = await accountContextService.GetAccountAsync();
        if (account == null) throw new NotFoundException();
        account.Email = dto.Email;
        dbContext.Accounts.Update(account);
        await dbContext.SaveChangesAsync();
        return account.MapToDto(account.Id);
    }
    
    public async Task<AccountDto> UpdateDescription(UpdateAccountDescriptionDto dto)
    {
        var account = await accountContextService.GetAccountAsync();
        var result = dbContext.Update(account).Entity.MapToDto(account.Id);
        await dbContext.SaveChangesAsync();
        return result;
    }
    
    public async Task<AccountDto> UpdatePassword(UpdateAccountPasswordDto dto)
    {
        var account = await accountContextService.GetAccountAsync();
        account.PasswordHash = passwordHasher.HashPassword(account, dto.Password);
        var result = dbContext.Update(account).Entity.MapToDto(account.Id);
        await dbContext.SaveChangesAsync();
        return result;
    }

    public Task<AccountDto> UpdateBackgroundPicture(UpdateAccountPictureDto dto)
    {
        // var account = await accountContextService.GetAccountAsync();
        // if (dto.File != null && !dto.File.ContentType.StartsWith("image")) throw new BadRequestException("invalid picture");
        // var fileExt = dto.File != null && dto.File.ContentType.EndsWith("gif") ? "gif" : "webp";
        // var bgName = $"{IdHasher.EncodeAccountId(account.Id)}-{DateTime.UtcNow.ToFileTimeUtc()}-bgp.{fileExt}";
        // var result = dbContext.Update(account).Entity.MapToDto(account.Id);
        // await dbContext.SaveChangesAsync();
        // return result;
        throw new NotImplementedException();
    }

    public async Task<AccountDto> UpdateProfilePicture(UpdateAccountPictureDto dto)
    {
        var account = await accountContextService.GetAccountAsync();
        if (dto.File != null && !dto.File.ContentType.StartsWith("image")) throw new BadRequestException("invalid picture");
        var fileExt = dto.File != null && dto.File.ContentType.EndsWith("gif") ? "gif" : "webp";
        var bgName = $"{IdHasher.EncodeAccountId(account.Id)}-{DateTime.UtcNow.ToFileTimeUtc()}-bgp.{fileExt}";
        account.ProfilePicUrl = Path.Combine("wwwroot", "accounts", "profile_pictures", $"{bgName}");
        var result = dbContext.Update(account).Entity.MapToDto(account.Id);
        await dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<bool> Delete(int id)
    {
        var currentUserAccount = await accountContextService.GetAccountAsync();
        logger.LogWarning("Account with Nickname: {AccountNickname} DELETE (SOFT) action invoked", currentUserAccount.Nickname);
        await AuthorizeAccountOperation(currentUserAccount, ResourceOperation.Delete ,"You have no rights to delete this account");
        
        var toDelete = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == id) ?? throw new NotFoundException($"Could not find account with id {id}");
        toDelete.IsDeleted = true;
        dbContext.Accounts.Update(currentUserAccount);
        await dbContext.SaveChangesAsync();
        logger.LogWarning($"Account with Nickname: {toDelete.Nickname}, Id: {toDelete.Id} DELETE (SOFT) action succeed");
        return toDelete.IsDeleted;
    }

    #region Private methods

    private async Task AuthorizeAccountOperation(Account account, ResourceOperation operation, string message)
    {
        var user = accountContextService.User;
        var authorizationResult = await authorizationService.AuthorizeAsync(user, account, new AccountOperationRequirement(operation));
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

    #endregion

}