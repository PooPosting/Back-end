using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountContextService _accountContextService;
    private readonly IAccountRepo _accountRepo;
    private readonly IPictureRepo _pictureRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly IAuthorizationService _authorizationService;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountService(
        IMapper mapper,
        ILogger<AccountService> logger,
        IAccountContextService accountContextService,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        ILikeRepo likeRepo,
        IAuthorizationService authorizationService,
        IPasswordHasher<Account> passwordHasher)
    {        
        _mapper = mapper;
        _logger = logger;
        _accountContextService = accountContextService;
        _accountRepo = accountRepo;
        _pictureRepo = pictureRepo;
        _likeRepo = likeRepo;
        _authorizationService = authorizationService;
        _passwordHasher = passwordHasher;
    }
        
    public AccountDto GetById(int id)
    {
        var account = _accountRepo.GetById(id);
        if (account is null || account.IsDeleted) throw new NotFoundException();
        var result = _mapper.Map<AccountDto>(account);
        AllowModifyItems(result);
        return result;
    }

    public PagedResult<AccountDto> GetAll(AccountQuery query)
    {
        var accounts = _accountRepo
            .SearchAll(
                query.PageSize * (query.PageNumber - 1),
                query.PageSize,
                query.SearchPhrase);

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts).ToList();
        AllowModifyItems(accountDtos);
        var result = new PagedResult<AccountDto>(accountDtos, query.PageSize, query.PageNumber);
        return result;
    }

    public List<LikeDto> GetAccLikes(int id)
    {
        if (_accountRepo.GetById(id) is null) throw new NotFoundException();
        
        var likes = _likeRepo.GetByLikerId(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);
        return likeDtos;
    }

    public bool Update(PutAccountDto dto)
    {
        var id = _accountContextService.GetAccountId();
        var account = _accountRepo.GetById(id);

        if (dto.Email is not null)
            account.Email = dto.Email;

        if (dto.Password is not null)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new BadRequestException("passwords are not the same");

            account.PasswordHash = _passwordHasher.HashPassword(account, dto.Password);
        }

        _accountRepo.Save();
        return true;
    }

    public bool Delete(int id)
    {
        var account = _accountRepo.GetById(id);
        if (account is null || account.IsDeleted) throw new NotFoundException();
        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE action invoked");
        AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account");
        
        _accountRepo.DeleteById(id);

        _logger.LogWarning($"Account with Nickname: {account.Nickname}, Id: {account.Id} DELETE (HIDE) action succeed");

        return true;
    }

    public bool DeleteAccPics(int id)
    {
        var account = _accountRepo.GetById(id);
        if (account is null || account.IsDeleted) throw new NotFoundException();

        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE ALL PICTURES action invoked");
        AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account's pictures");

        foreach (var pic in account.Pictures)
        {
            _pictureRepo.DeleteById(pic.Id);
        }

        _logger.LogWarning($"Account with Nickname: {account.Nickname}, Id: {account.Id} DELETE (HIDE) ALL PICTURES action succeed");
        
        return true;
    }

    #region Private methods

    private void AllowModifyItems(List<AccountDto> accounts)
    {
        if (_accountContextService.TryGetAccountId() is null) return;
        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();
        foreach (var account in accounts)
        {
            foreach (var item in account.Pictures)
            {
                if (item.AccountId == accountId)
                {
                    item.IsModifiable = true;
                }
                else if (accountRole == 3)
                {
                    item.IsAdminModifiable = true;
                }

                foreach (var comment in item.Comments)
                {
                    if (comment.AccountId == accountId)
                    {
                        comment.IsModifiable = true;
                    }
                    else if (accountRole == 3)
                    {
                        item.IsAdminModifiable = true;
                    }
                }

                if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
                {
                    item.LikeState = LikeState.Liked;
                }
                if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
                {
                    item.LikeState = LikeState.DisLiked;
                }
            }
        }
    }

    private void AllowModifyItems(AccountDto account)
    {
        if (_accountContextService.TryGetAccountId() is null) return;
        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();
        foreach (var item in account.Pictures)
        {
            if (item.AccountId == accountId)
            {
                item.IsModifiable = true;
            }
            else if (accountRole == 3)
            {
                item.IsAdminModifiable = true;
            }

            foreach (var comment in item.Comments)
            {
                if (comment.AccountId == accountId)
                {
                    comment.IsModifiable = true;
                }
                else if (accountRole == 3)
                {
                    item.IsAdminModifiable = true;
                }
            }

            if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
            {
                item.LikeState = LikeState.Liked;
            }
            if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
            {
                item.LikeState = LikeState.DisLiked;
            }
        }
    }

    private void AuthorizeAccountOperation(Account account, ResourceOperation operation, string message)
    {
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, account, new AccountOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

    #endregion

}