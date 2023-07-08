using System.Globalization;
using AutoMapper;
using FluentValidation;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PooPosting.Api.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Enums;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Models.Dtos;
using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Validators;

namespace PooPosting.Api.Services;

public class HttpAccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly ILogger<HttpAccountService> _logger;
    private readonly IAccountContextService _accountContextService;
    private readonly IAccountRepo _accountRepo;
    private readonly IAuthorizationService _authorizationService;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public HttpAccountService(
        IMapper mapper,
        ILogger<HttpAccountService> logger,
        IAccountContextService accountContextService,
        IAccountRepo accountRepo,
        IAuthorizationService authorizationService,
        IPasswordHasher<Account> passwordHasher)
    {        
        _mapper = mapper;
        _logger = logger;
        _accountContextService = accountContextService;
        _accountRepo = accountRepo;
        _authorizationService = authorizationService;
        _passwordHasher = passwordHasher;
    }
        
    public async Task<AccountDto> GetById(
        int id
        )
    {
        var account = await _accountRepo.GetByIdAsync(id);
        if (account is null || account.IsDeleted) throw new NotFoundException();
        var result = _mapper.Map<AccountDto>(account);
        return result;
    }

    public async Task<PagedResult<AccountDto>> GetAll(
        CustomQuery query
        )
    {
        var accounts = await _accountRepo
            .SearchAllAsync(
                query.PageSize * (query.PageNumber - 1),
                query.PageSize,
                query.SearchPhrase
                );

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts).ToList();
        var result = new PagedResult<AccountDto>(
            accountDtos,
            query.PageNumber,
            query.PageSize,
            await _accountRepo.CountAccountsAsync(
                a => string.IsNullOrEmpty(query.SearchPhrase) || a.Nickname.ToLower().Contains(query.SearchPhrase.ToLower())
                )
            );
        return result;
    }

    public async Task<AccountDto> UpdateEmail(
        UpdateAccountEmailDto dto
        )
    {
        var account = await _accountContextService.GetAccountAsync();
        account.Email = dto.Email;
        account = await _accountRepo.UpdateAsync(account);
        return _mapper.Map<AccountDto>(account);
    }
    public async Task<AccountDto> UpdateDescription(
        UpdateAccountDescriptionDto dto
        )
    {
        var account = await _accountContextService.GetAccountAsync();
        account.AccountDescription = dto.Description;
        account = await _accountRepo.UpdateAsync(account);
        return _mapper.Map<AccountDto>(account);
    }
    public async Task<AccountDto> UpdatePassword(
        UpdateAccountPasswordDto dto
        )
    {
        var account = await _accountContextService.GetAccountAsync();
        account.PasswordHash = _passwordHasher.HashPassword(account, dto.Password);
        account = await _accountRepo.UpdateAsync(account);
        return _mapper.Map<AccountDto>(account);
    }

    public async Task<AccountDto> UpdateBackgroundPicture(
        UpdateAccountPictureDto dto
        )
    {
        var account = await _accountContextService.GetAccountAsync();
        if (!dto.File.ContentType.StartsWith("image")) throw new BadRequestException("invalid picture");
        var fileExt = dto.File.ContentType.EndsWith("gif") ? "gif" : "webp";
        var bgName = $"{IdHasher.EncodeAccountId(account.Id)}-{DateTime.Now.ToFileTimeUtc()}-bgp.{fileExt}";
        var rootPath = Directory.GetCurrentDirectory();
        var fullBgPath = Path.Combine(rootPath, "wwwroot", "accounts", "background_pictures", $"{bgName}");

        try
        {
            using var ms = new MemoryStream();
            await dto.File.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            var result = await NsfwClassifier.ClassifyAsync(fileBytes, CancellationToken.None);

            var errors = new List<string>();

            if (result.Adult > Likelihood.Possible) errors.Add("Adult");
            if (result.Racy > Likelihood.Likely) errors.Add("Racy");
            if (result.Medical > Likelihood.Likely) errors.Add("Medical");
            if (result.Violence > Likelihood.Likely) errors.Add("Violence");

            if (errors.Any())
            {
                throw new BadRequestException($"inappropriate picture: [{string.Join(", ", errors)}]");
            }

            await using var stream = new FileStream(fullBgPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            await dto.File.CopyToAsync(stream);
            await stream.DisposeAsync();
        }
        catch (Exception)
        {
            if (File.Exists(fullBgPath)) File.Delete(fullBgPath);
            throw;
        }
        account.BackgroundPicUrl = Path.Combine("wwwroot", "accounts", "background_pictures", $"{bgName}");
        account = await _accountRepo.UpdateAsync(account);
        return _mapper.Map<AccountDto>(account);
    }

    public async Task<AccountDto> UpdateProfilePicture(
        UpdateAccountPictureDto dto
        )
    {
        var account = await _accountContextService.GetAccountAsync();
        if (!dto.File.ContentType.StartsWith("image")) throw new BadRequestException("invalid picture");
        var fileExt = dto.File.ContentType.EndsWith("gif") ? "gif" : "webp";
        var pfpName = $"{IdHasher.EncodeAccountId(account.Id)}-{DateTime.Now.ToFileTimeUtc()}-pfp.{fileExt}";
        var rootPath = Directory.GetCurrentDirectory();
        var fullPfpPath = Path.Combine(rootPath, "wwwroot", "accounts", "profile_pictures", $"{pfpName}");

        try
        {
            using var ms = new MemoryStream();
            await dto.File.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            var result = await NsfwClassifier.ClassifyAsync(fileBytes, CancellationToken.None);

            var errors = new List<string>();

            if (result.Adult > Likelihood.Possible) errors.Add("Adult");
            if (result.Racy > Likelihood.Likely) errors.Add("Racy");
            if (result.Medical > Likelihood.Likely) errors.Add("Medical");
            if (result.Violence > Likelihood.Likely) errors.Add("Violence");

            if (errors.Any())
            {
                throw new BadRequestException($"inappropriate picture: [{string.Join(", ", errors)}]");
            }

            await using var stream = new FileStream(fullPfpPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            await dto.File.CopyToAsync(stream);
            await stream.DisposeAsync();
        }
        catch (Exception)
        {
            if (File.Exists(fullPfpPath)) File.Delete(fullPfpPath);
            throw;
        }
        account.ProfilePicUrl = Path.Combine("wwwroot", "accounts", "profile_pictures", $"{pfpName}");
        account = await _accountRepo.UpdateAsync(account);
        return _mapper.Map<AccountDto>(account);
    }

    public async Task<bool> Delete(
        int id
        )
    {
        var account = await _accountContextService.GetTrackedAccountAsync();
        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE action invoked");
        await AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account");

        account.IsDeleted = true;
        await _accountRepo.UpdateAsync(account);
        _logger.LogWarning($"Account with Nickname: {account.Nickname}, Id: {account.Id} DELETE (HIDE) action succeed");
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