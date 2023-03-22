using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    private readonly ILikeRepo _likeRepo;

    public LikeService(
        IMapper mapper,
        ILikeRepo likeRepo
        )
    {
        _mapper = mapper;
        _likeRepo = likeRepo;
    }

    public async Task<PagedResult<LikeDto>> GetLikesByPictureId(
        Query query,
        int picId
    )
    {
        var likes = await _likeRepo
            .GetByPictureIdAsync(
                picId,
                query.PageSize * (query.PageNumber - 1),
                query.PageSize
                );

        return new PagedResult<LikeDto>(
            _mapper.Map<IEnumerable<LikeDto>>(likes),
            query.PageNumber,
            query.PageSize,
            await _likeRepo.CountLikesAsync(l => l.PictureId == picId)
            );
    }

}