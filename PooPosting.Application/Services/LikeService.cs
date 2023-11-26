using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Mappers;
using PooPosting.Application.Models;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Domain.DbContext;

namespace PooPosting.Application.Services;

public class LikeService(
    PictureDbContext dbContext
    ) : ILikeService
{
    public async Task<PagedResult<LikeDto>> GetLikesByPictureId(
        Query query,
        int picId
    )
    {
        var likes = await dbContext.Likes
            .Where(l => l.PictureId == picId)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ProjectToDto()
            .ToListAsync();

        return new PagedResult<LikeDto>(
            likes,
            query.PageNumber,
            query.PageSize,
            await dbContext.Likes.CountAsync(l => l.PictureId == picId)
            );
    }

}