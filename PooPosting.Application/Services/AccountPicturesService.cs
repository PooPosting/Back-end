using PooPosting.Application.Mappers;
using PooPosting.Application.Models.Dtos.Picture.Out;
using PooPosting.Application.Services.Helpers;
using PooPosting.Domain.DbContext;
using PooPosting.Domain.DbContext.Interfaces;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Application.Services;

public class AccountPicturesService(PictureDbContext dbContext)
{
    public async Task<PagedResult<PictureDto>> GetPaged(IPaginationParameters paginationParameters, string accountId)
    {
        return await dbContext.Pictures
            .OrderByDescending(p => p.PictureAdded)
            .Where(p => p.AccountId == IdHasher.DecodeAccountId(accountId))
            .ProjectToDto()
            .Paginate(paginationParameters);
    }
    
    public async Task<PagedResult<PictureDto>> GetLikedPaged(IPaginationParameters paginationParameters, string accountId)
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