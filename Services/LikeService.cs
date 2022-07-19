using AutoMapper;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos.Like;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class LikeService : ILikeService
{
    private readonly IMapper _mapper;
    private readonly ILikeRepo _likeRepo;
    private readonly IPictureRepo _pictureRepo;

    public LikeService(
        IMapper mapper,
        ILikeRepo likeRepo,
        IPictureRepo pictureRepo
        )
    {
        _mapper = mapper;
        _likeRepo = likeRepo;
        _pictureRepo = pictureRepo;
    }

    public async Task<IEnumerable<LikeDto>> GetLikesByPicture(
        int id
    )
    {
        if (await _pictureRepo.GetByIdAsync(id) is null) throw new NotFoundException();
        var likes = await _likeRepo.GetByLikedIdAsync(id);
        var likeDtos = _mapper.Map<List<LikeDto>>(likes);

        return likeDtos;
    }
}