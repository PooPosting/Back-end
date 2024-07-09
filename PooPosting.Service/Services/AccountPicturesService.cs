using PooPosting.Application.Mappers;
using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Domain.DbContext;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Application.Services;

public class AccountPicturesService(PictureDbContext dbContext)
    : IAccountPicturesService
{
    public async Task<PagedResult<PictureDto>> GetPaged(PaginationParameters paginationParameters, string accountId)
    {
        return await dbContext.Pictures
            .OrderByDescending(p => p.PictureAdded)
            .Where(p => p.AccountId == IdHasher.DecodeAccountId(accountId))
            .ProjectToDto()
            .Paginate(paginationParameters);
    }
    
    public async Task<PagedResult<PictureDto>> GetLikedPaged(PaginationParameters paginationParameters, string accountId)
    {
        return await dbContext.Likes
            .OrderByDescending(l => l.Liked)
            .ThenByDescending(l => l.Id)
            .Where(l => l.AccountId == IdHasher.DecodeAccountId(accountId))
            .Select(l => l.Picture)
            .ProjectToDto()
            .Paginate(paginationParameters);
    }
}