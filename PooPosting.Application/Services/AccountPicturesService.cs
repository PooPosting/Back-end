using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Mappers;
using PooPosting.Application.Models;
using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Domain.DbContext;

namespace PooPosting.Application.Services;

public class AccountPicturesService(
        PictureDbContext dbContext,
        IAccountContextService accountContextService
        )
    : IAccountPicturesService
{
    public async Task<PagedResult<PictureDto>> GetPaged(Query query, string accountId)
    {
        var accountsQueryable= dbContext.Pictures
            .Where(a => a.AccountId == IdHasher.DecodeAccountId(accountId))
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize);

        var count = await accountsQueryable.CountAsync();
        
        var currAccId = accountContextService.TryGetAccountId();
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
        var picsQueryable = dbContext.Likes
            .OrderByDescending(l => l.Liked)
            .ThenByDescending(l => l.Id)
            .Where(l => l.AccountId == IdHasher.DecodeAccountId(accountId))
            .Select(l => l.Picture)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize);
        
        var count = await picsQueryable.CountAsync();
        
        var currAccId = accountContextService.TryGetAccountId();
        var pictureDtos= await picsQueryable
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