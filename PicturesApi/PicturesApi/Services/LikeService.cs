using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Exceptions;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos.Like;
using PicturesAPI.Models.Queries;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

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