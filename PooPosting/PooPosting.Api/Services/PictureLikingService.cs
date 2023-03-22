using AutoMapper;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Helpers.Interfaces;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Services;

public class PictureLikingService : IPictureLikingService
{
    private readonly IPictureRepo _pictureRepo;
    private readonly IMapper _mapper;
    private readonly ILikeHelper _likeHelper;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeHelper likeHelper,
        IPictureRepo pictureRepo,
        IMapper mapper,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _mapper = mapper;
        _likeHelper = likeHelper;
        _accountContextService = accountContextService;
    }
    
    public async Task<PictureDto> Like(int pictureId)
    {
        var picture = (await _pictureRepo.GetByIdAsync(pictureId)) ?? throw new NotFoundException();
        var accountId = _accountContextService.GetAccountId();

        var pictureResult = await _likeHelper.LikeAsync(pictureId, accountId);
        var mappedResult = _mapper.Map<PictureDto>(pictureResult);
        return mappedResult;
    }

    public async Task<PictureDto> DisLike(int pictureId)
    {
        var picture = (await _pictureRepo.GetByIdAsync(pictureId)) ?? throw new NotFoundException();
        var accountId = _accountContextService.GetAccountId();

        var pictureResult = await _likeHelper.DislikeAsync(pictureId, accountId);
        var mappedResult = _mapper.Map<PictureDto>(pictureResult);
        return mappedResult;
    }

}