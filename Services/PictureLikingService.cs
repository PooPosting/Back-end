using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Helpers.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PictureLikingService : IPictureLikingService
{
    private readonly IPictureRepo _pictureRepo;
    private readonly IModifyAllower _modifyAllower;
    private readonly ITagRepo _tagRepo;
    private readonly IMapper _mapper;
    private readonly ILikeRepo _likeRepo;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeRepo likeRepo,
        IPictureRepo pictureRepo,
        IModifyAllower modifyAllower,
        ITagRepo tagRepo,
        IMapper mapper,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _modifyAllower = modifyAllower;
        _tagRepo = tagRepo;
        _mapper = mapper;
        _likeRepo = likeRepo;
        _accountContextService = accountContextService;
    }
    
    public async Task<PictureDto> Like(int id)
    {
        var picture = await _pictureRepo.GetByIdAsync(id) ?? throw new NotFoundException();
        var account = await _accountContextService.GetAccountAsync();
        var like = await _likeRepo.GetByLikerIdAndLikedIdAsync(account.Id, picture.Id);


        var tags = await _tagRepo.GetTagsByPictureIdAsync(picture.Id);
        if (like is not null)
        {
            // like exists and (IsLike == true)
            if (like.IsLike)
            {
                await _likeRepo.DeleteByIdAsync(like.Id);
                foreach (var tag in tags)
                {
                    await _tagRepo.TryDeleteAccountLikedTagAsync(account, tag);
                }
                await _pictureRepo.UpdatePicScoreAsync(picture);

                var result = _mapper.Map<PictureDto>(picture);
                _modifyAllower.UpdateItems(result);
                return result;
            }
            // like exists and (IsLike == false)
            else
            {
                like.IsLike = !like.IsLike;
                await _likeRepo.UpdateAsync(like);
                foreach (var tag in tags)
                {
                    await _tagRepo.TryInsertAccountLikedTagAsync(account, tag);
                }
                await _pictureRepo.UpdatePicScoreAsync(picture);

                var result = _mapper.Map<PictureDto>(picture);
                _modifyAllower.UpdateItems(result);
                return result;
            }
        }
        // like does not exist
        else
        {
            await _likeRepo.InsertAsync(
                new Like()
                {
                    Liked = picture,
                    Liker = account,
                    IsLike = true
                });
            foreach (var tag in tags)
            {
                await _tagRepo.TryInsertAccountLikedTagAsync(account, tag);
            }
            await _pictureRepo.UpdatePicScoreAsync(picture);

            var result = _mapper.Map<PictureDto>(picture);
            _modifyAllower.UpdateItems(result);
            return result;
        }

    }

    public async Task<PictureDto> DisLike(int id)
    {
        var picture = await _pictureRepo.GetByIdAsync(id) ?? throw new NotFoundException();
        var account = await _accountContextService.GetAccountAsync();
        var like = await _likeRepo.GetByLikerIdAndLikedIdAsync(account.Id, picture.Id);

        if (like is not null)
        {
            // like exists and (IsLike == false)
            if (like.IsLike == false)
            {
                await _likeRepo.DeleteByIdAsync(like.Id);
                await _pictureRepo.UpdatePicScoreAsync(picture);

                var result = _mapper.Map<PictureDto>(picture);
                _modifyAllower.UpdateItems(result);
                return result;
            }
            // like exists and (IsLike == true)
            else
            {
                like.IsLike = !like.IsLike;
                await _likeRepo.UpdateAsync(like);
                var tags = await _tagRepo.GetTagsByPictureIdAsync(picture.Id);
                foreach (var tag in tags)
                {
                    await _tagRepo.TryDeleteAccountLikedTagAsync(account, tag);
                }
                await _pictureRepo.UpdatePicScoreAsync(picture);

                var result = _mapper.Map<PictureDto>(picture);
                _modifyAllower.UpdateItems(result);
                return result;
            }
        }
        // does not exist
        else
        {
            await _likeRepo.InsertAsync(
                new Like()
                {
                    Liked = picture,
                    Liker = account,
                    IsLike = false
                });
            await _pictureRepo.UpdatePicScoreAsync(picture);

            var result = _mapper.Map<PictureDto>(picture);
            _modifyAllower.UpdateItems(result);
            return result;
        }
    }

}