using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;
using PooPosting.Api.Mappers;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services;

public class AccountPicturesService : IAccountPicturesService
{
    private readonly PictureDbContext _dbContext;
    private readonly IAccountContextService _accountContextService;

    public AccountPicturesService(
        PictureDbContext dbContext,
        IAccountContextService accountContextService
        )
    {
        _dbContext = dbContext;
        _accountContextService = accountContextService;
    }

    public async Task<PagedResult<PictureDto>> GetPaged(Query query, string accountId)
    {
        var accountsQueryable= _dbContext.Pictures
            .Where(a => a.AccountId == IdHasher.DecodeAccountId(accountId))
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize);

        var count = await accountsQueryable.CountAsync();
        
        var currAccId = _accountContextService.TryGetAccountId();
        var pictureDtos     = await accountsQueryable
            .ProjectToDto(currAccId)
            .ToListAsync();

        var result = new PagedResult<PictureDto>(
            pictureDtos    ,
            query.PageNumber,
            query.PageSize,
            count
        );
        return result;
    }
    
    public async Task<PagedResult<PictureDto>> GetLikedPaged(Query query, string accountId)
    {
        var accountsQueryable= _dbContext.Pictures
            .Where(p => p.Likes.Any(l => l.AccountId == IdHasher.DecodeAccountId(accountId)))
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize);

        var count = await accountsQueryable.CountAsync();
        
        var currAccId = _accountContextService.TryGetAccountId();
        var pictureDtos= await accountsQueryable
            .ProjectToDto(currAccId)
            .ToListAsync();

        var result = new PagedResult<PictureDto>(
            pictureDtos    ,
            query.PageNumber,
            query.PageSize,
            count
        );
        return result;
    }
}