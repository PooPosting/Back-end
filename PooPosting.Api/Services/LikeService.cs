using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Exceptions;

namespace PooPosting.Api.Services;

public class LikeService : ILikeService
{
    private readonly IMapper _mapper;
    private readonly PictureDbContext _dbContext;

    public LikeService(
        IMapper mapper,
        PictureDbContext dbContext
        )
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<PagedResult<LikeDto>> GetLikesByPictureId(
        Query query,
        int picId
    )
    {
        var likes = await _dbContext.Likes
            .Where(l => l.PictureId == picId)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<LikeDto>(
            _mapper.Map<IEnumerable<LikeDto>>(likes),
            query.PageNumber,
            query.PageSize,
            await _dbContext.Likes.CountAsync(l => l.PictureId == picId)
            );
    }

}