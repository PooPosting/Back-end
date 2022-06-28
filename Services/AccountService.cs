﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Validators;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Helpers.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountContextService _accountContextService;
    private readonly IModifyAllower _modifyAllower;
    private readonly IAccountRepo _accountRepo;
    private readonly IPictureRepo _pictureRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly IAuthorizationService _authorizationService;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountService(
        IMapper mapper,
        ILogger<AccountService> logger,
        IAccountContextService accountContextService,
        IModifyAllower modifyAllower,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        ILikeRepo likeRepo,
        IAuthorizationService authorizationService,
        IPasswordHasher<Account> passwordHasher)
    {        
        _mapper = mapper;
        _logger = logger;
        _accountContextService = accountContextService;
        _modifyAllower = modifyAllower;
        _accountRepo = accountRepo;
        _pictureRepo = pictureRepo;
        _likeRepo = likeRepo;
        _authorizationService = authorizationService;
        _passwordHasher = passwordHasher;
    }
        
    public async Task<AccountDto> GetById(int id)
    {
        var account = await _accountRepo.GetByIdAsync(id);
        if (account is null || account.IsDeleted) throw new NotFoundException();
        var result = _mapper.Map<AccountDto>(account);
        _modifyAllower.UpdateItems(result);
        return result;
    }

    public async Task<PagedResult<AccountDto>> GetAll(AccountQuery query)
    {
        var accounts = await _accountRepo
            .SearchAllAsync(
                query.PageSize * (query.PageNumber - 1),
                query.PageSize,
                query.SearchPhrase);

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts).ToList();
        _modifyAllower.UpdateItems(accountDtos);
        var result = new PagedResult<AccountDto>(
            accountDtos,
            query.PageSize,
            query.PageNumber,
            await _accountRepo.CountAccountsAsync(
                a => string.IsNullOrEmpty(query.SearchPhrase) || a.Nickname.ToLower().Contains(query.SearchPhrase.ToLower())
            ));
        return result;
    }

    public async Task<List<LikeDto>> GetAccLikes(int id)
    {
        if (await _accountRepo.GetByIdAsync(id) is null) throw new NotFoundException();
        
        var likes = await _likeRepo.GetByLikerIdAsync(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);
        return likeDtos;
    }

    public async Task<AccountDto> Update(UpdateAccountDto dto)
    {
        var validationResult = await StaticValidator.Validate(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var rootPath = Directory.GetCurrentDirectory();
        var account = await _accountContextService.GetAccountAsync();

        var bgName = $"{IdHasher.EncodeAccountId(account.Id)}-bgp.webp";
        var fullBgPath = Path.Combine(rootPath, "wwwroot", "accounts", "background_pictures", $"{bgName}");

        var picName = $"{IdHasher.EncodeAccountId(account.Id)}-pfp.webp";
        var fullPicPath = Path.Combine(rootPath, "wwwroot", "accounts", "profile_pictures", $"{picName}");

        try
        {
            if (dto.BackgroundPic is not null)
            {
                if (dto.BackgroundPic is not { Length: > 0 }) throw new BadRequestException("invalid picture");
                account.BackgroundPicUrl = Path.Combine("wwwroot", "accounts", "background_pictures", $"{bgName}");

                await using var stream = new FileStream(fullBgPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                await dto.BackgroundPic.CopyToAsync(stream);
                await stream.DisposeAsync();
            }
            if (dto.ProfilePic is not null)
            {
                if (dto.ProfilePic is not { Length: > 0 }) throw new BadRequestException("invalid picture");
                account.ProfilePicUrl = Path.Combine("wwwroot", "accounts", "profile_pictures", $"{picName}");

                await using var stream = new FileStream(fullPicPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                await dto.ProfilePic.CopyToAsync(stream);
                await stream.DisposeAsync();
            }

            if (dto.Email is not null) account.Email = dto.Email;
            if (dto.Password is not null)
            {
                if (dto.Password != dto.ConfirmPassword)
                    throw new BadRequestException("passwords are not the same");
                account.PasswordHash = _passwordHasher.HashPassword(account, dto.Password);
            }

            account = await _accountRepo.UpdateAsync(account);

            var accountDto = _mapper.Map<AccountDto>(account);
            _modifyAllower.UpdateItems(accountDto);
            return accountDto;
        }
        catch (Exception)
        {
            if (File.Exists(fullBgPath)) File.Delete(fullBgPath);
            if (File.Exists(fullPicPath)) File.Delete(fullPicPath);
            throw;
        }
    }

    public async Task<bool> Delete(int id)
    {
        var account = await _accountContextService.GetAccountAsync();
        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE action invoked");
        await AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account");

        var result = await _accountRepo.TryDeleteByIdAsync(id);
        _logger.LogWarning($"Account with Nickname: {account.Nickname}, Id: {account.Id} DELETE (HIDE) action succeed");
        return result;
    }

    public async Task<IEnumerable<PictureDto>> DeleteAccPics(int id)
    {
        var account = await _accountRepo.GetByIdAsync(id);
        if (account is null || account.IsDeleted) throw new NotFoundException();

        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE ALL PICTURES action invoked");
        await AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account's pictures");

        var pictures = new List<Picture>();
        foreach (var pic in account.Pictures)
        {
            if (await _pictureRepo.DeleteByIdAsync(pic.Id))
            {
                pictures.Add(pic);
            }
        }
        _logger.LogWarning($"Account with Nickname: {account.Nickname}, Id: {account.Id} DELETE (HIDE) ALL PICTURES action succeed");
        
        var pictureDtos = _mapper.Map<List<PictureDto>>(pictures);
        _modifyAllower.UpdateItems(pictureDtos);
        return pictureDtos;
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