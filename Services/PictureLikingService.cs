using AutoMapper;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PictureLikingService : IPictureLikingService
{
    private readonly IPictureRepo _pictureRepo;
    private readonly IMapper _mapper;
    private readonly ILikeRepo _likeRepo;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeRepo likeRepo,
        IPictureRepo pictureRepo,
        ITagRepo tagRepo,
        IMapper mapper,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _mapper = mapper;
        _likeRepo = likeRepo;
        _accountContextService = accountContextService;
    }
    
    public async Task<PictureDto> Like(int pictureId)
    {
        var picture = (await _pictureRepo.GetByIdAsync(pictureId)) ?? throw new NotFoundException();
        var accountId = _accountContextService.GetAccountId();

        var pictureResult = await _likeRepo.LikeAsync(pictureId, accountId);
        var mappedResult = _mapper.Map<PictureDto>(pictureResult);
        return mappedResult;
    }

    public async Task<PictureDto> DisLike(int pictureId)
    {
        var picture = (await _pictureRepo.GetByIdAsync(pictureId)) ?? throw new NotFoundException();
        var accountId = _accountContextService.GetAccountId();

        var pictureResult = await _likeRepo.DislikeAsync(pictureId, accountId);
        var mappedResult = _mapper.Map<PictureDto>(pictureResult);
        return mappedResult;
    }

}