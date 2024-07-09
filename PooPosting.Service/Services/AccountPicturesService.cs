using PooPosting.Data.DbContext;
using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Mappers;
using PooPosting.Service.Models.Dtos.Picture;
using PooPosting.Service.Services.Helpers;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Service.Services;

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