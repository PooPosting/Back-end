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
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

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
        if (account is null || account.IsDeleted) throw new NotFoundException("account not found");
        var result = _mapper.Map<AccountDto>(account);
        return result;
    }

    public PagedResult<AccountDto> GetAll(AccountQuery query)
    {
        #region Update this

        var baseQuery = _accountRepo.GetAll()
            .Where(a => query.SearchPhrase == null || a.Nickname.ToLower().Contains(query.SearchPhrase.ToLower()))
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => _pictureRepo.GetByAccountId(a.Id).Count())
            .ThenByDescending(a => _pictureRepo.GetByAccountId(a.Id).Sum(picture => picture.Likes.Count))
            .ToList();

        var accounts = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        #endregion

        if (accounts.Count == 0) throw new NotFoundException("accounts not found");
        
        var resultCount = baseQuery.Count;
        var accountDtos = _mapper.Map<List<AccountDto>>(accounts).ToList();
        var result = new PagedResult<AccountDto>(accountDtos, resultCount, query.PageSize, query.PageNumber);
        
        return result;
    }

    public List<LikeDto> GetAccLikes(int id)
    {
        if (_accountRepo.GetById(id) is null) throw new NotFoundException("account not found");
        
        var likes = _likeRepo.GetByLikerId(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);
        return likeDtos;
    }

    // public string GetLikedTags()
    // {
    //     var id = _accountContextService.GetAccountId();
    //     return _accountRepo.GetById(id).LikedTags;
    // }
    
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
        if (account is null || account.IsDeleted) throw new NotFoundException("account not found");
        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE action invoked");
        AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account");
        
        _accountRepo.DeleteById(id);

        _logger.LogWarning($"Account with Nickname: {account.Nickname}, Id: {account.Id} DELETE (HIDE) action succeed");

        return true;
    }

    public bool DeleteAccPics(int id)
    {
        var account = _accountRepo.GetById(id);
        if (account is null || account.IsDeleted) throw new NotFoundException("account not found");

        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE ALL PICTURES action invoked");
        AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account's pictures");

        foreach (var pic in account.Pictures)
        {
            _pictureRepo.DeleteById(pic.Id);
        }

        _logger.LogWarning($"Account with Nickname: {account.Nickname}, Id: {account.Id} DELETE (HIDE) ALL PICTURES action succeed");
        
        return true;
    }

    private void AuthorizeAccountOperation(Account account, ResourceOperation operation, string message)
    {
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, account, new AccountOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

}