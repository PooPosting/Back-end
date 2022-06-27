using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PictureLikingService : IPictureLikingService
{
    private readonly IPictureRepo _pictureRepo;
    private readonly ITagRepo _tagRepo;
    private readonly ILikeRepo _likeRepo;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeRepo likeRepo,
        IPictureRepo pictureRepo,
        ITagRepo tagRepo,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _tagRepo = tagRepo;
        _likeRepo = likeRepo;
        _accountContextService = accountContextService;
    }
    
    public async Task<LikeState> Like(int id)
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
                return LikeState.Deleted;
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
                return LikeState.Liked;
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
            return LikeState.Liked;
        }

    }

    public async Task<LikeState> DisLike(int id)
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
                return LikeState.Deleted;
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
                return LikeState.DisLiked;
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
            return LikeState.DisLiked;
        }
    }

}