using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

    public AccountService(
        IMapper mapper,
        ILogger<AccountService> logger,
        IAccountContextService accountContextService,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        ILikeRepo likeRepo,
        IAuthorizationService authorizationService)
    {        
        _mapper = mapper;
        _logger = logger;
        _accountContextService = accountContextService;
        _accountRepo = accountRepo;
        _pictureRepo = pictureRepo;
        _likeRepo = likeRepo;
        _authorizationService = authorizationService;
    }
        
    public AccountDto GetById(Guid id)
    {
        var account = _accountRepo.GetAccountById(id, DbInclude.Include);
        if (account is null || account.IsDeleted) throw new NotFoundException("account not found");
        var result = _mapper.Map<AccountDto>(account);
        
        return result;
    }

    public PagedResult<AccountDto> GetAll(AccountQuery query)
    {
        var baseQuery = _accountRepo.GetAccounts(DbInclude.Raw)
            .Where(a => query.SearchPhrase == null || a.Nickname.ToLower().Contains(query.SearchPhrase.ToLower()))
            .Where(a => !a.IsDeleted)
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

    public List<LikeDto> GetAccLikes(Guid id)
    {
        if (!_accountRepo.Exists(id)) throw new NotFoundException("account not found");
        
        var likes = _likeRepo.GetLikesByLiker(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);
        return likeDtos;
    }

    public string GetLikedTags()
    {
        var id = Guid.Parse(_accountContextService.GetAccountId!);
        if (!_accountRepo.Exists(id)) throw new InvalidAuthTokenException();
        var tags = _accountRepo.GetLikedTags(id);

        return tags;
    }
    
    public bool Update(PutAccountDto dto)
    {
        var id = Guid.Parse(_accountContextService.GetAccountId!);
        if (!_accountRepo.Exists(id)) throw new InvalidAuthTokenException();
        _accountRepo.UpdateAccount(dto, id);
        return true;
    }

    public bool Delete(Guid id)
    {
        var account = _accountRepo.GetAccountById(id, DbInclude.Raw);
        if (account is null || account.IsDeleted) throw new NotFoundException("account not found");
        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE action invoked");
        AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account");
        
        _accountRepo.DeleteAccount(id);

        _logger.LogWarning(
            "Account with " +
            $"Nickname: {account.Nickname}, " +
            $"Id: {account.Id} " +
            "DELETE action succeed");
        return true;
    }

    public bool DeleteAccountPictures(Guid id)
    {
        var account = _accountRepo.GetAccountById(id, DbInclude.Include);
        if (account is null || account.IsDeleted) throw new NotFoundException("account not found");
        _logger.LogWarning($"Account with Nickname: {account.Nickname} DELETE ALL PICTURES action invoked");
        AuthorizeAccountOperation(account, ResourceOperation.Delete ,"You have no rights to delete this account's pictures");

        foreach (var pic in account.Pictures)
        {
            _pictureRepo.DeletePicture(pic);
        }
        _logger.LogWarning(
            "Account with " +
            $"Nickname: {account.Nickname}, " +
            $"Id: {account.Id} " +
            "DELETE ALL PICTURES action succeed");
        
        return true;
    }

    private void AuthorizeAccountOperation(Account account, ResourceOperation operation, string message)
    {
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, account, new AccountOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }

}