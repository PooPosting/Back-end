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
    private readonly IAccountRepo _accountRepo;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeRepo likeRepo,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        ITagRepo tagRepo,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _tagRepo = tagRepo;
        _likeRepo = likeRepo;
        _accountRepo = accountRepo;
        _accountContextService = accountContextService;
    }
    
    public LikeState Like(int id)
    {
        var picture = _pictureRepo.GetById(id);
        var accountId = _accountContextService.GetAccountId();

        if (picture is null) throw new NotFoundException("picture not found");

        // set up like/dislike logic (database liked tags insertion/drop)
        var account = _accountRepo.GetById(accountId);
        var like = _likeRepo.GetByLikerIdAndLikedId(account.Id, picture.Id);


        var tags = _tagRepo.GetTagsByPictureId(picture.Id);
        if (like is not null)
        {
            // like exists and (IsLike == true)
            if (like.IsLike)
            {
                _likeRepo.DeleteById(like.Id);
                foreach (var tag in tags)
                {
                    _tagRepo.TryDeleteAccountLikedTag(account, tag);
                }
                _likeRepo.Save();
                _pictureRepo.UpdatePicScore(picture);
                return LikeState.Deleted;
            }
            // like exists and (IsLike == false)
            else
            {
                like.IsLike = !like.IsLike;
                _likeRepo.Update(like);
                foreach (var tag in tags)
                {
                    _tagRepo.TryInsertAccountLikedTag(account, tag);
                }
                _likeRepo.Save();
                _pictureRepo.UpdatePicScore(picture);
                return LikeState.Liked;
            }
        }
        // like does not exist
        else
        {
            _likeRepo.Insert(
                new Like()
                {
                    Liked = picture,
                    Liker = account,
                    IsLike = true
                });
            foreach (var tag in tags)
            {
                _tagRepo.TryInsertAccountLikedTag(account, tag);
            }
            _likeRepo.Save();
            _pictureRepo.UpdatePicScore(picture);
            return LikeState.Liked;
        }

    }

    public LikeState DisLike(int id)
    {
        var picture = _pictureRepo.GetById(id);
        var accountId = _accountContextService.GetAccountId();
        if (picture is null) throw new NotFoundException("picture not found");

        // set up like/dislike logic (database liked tags insertion/drop)
        var account = _accountRepo.GetById(accountId);
        var like = _likeRepo.GetByLikerIdAndLikedId(account.Id, picture.Id);

        if (like is not null)
        {
            // like exists and (IsLike == false)
            if (like.IsLike == false)
            {
                _likeRepo.DeleteById(like.Id);
                _pictureRepo.UpdatePicScore(picture);
                _likeRepo.Save();
                return LikeState.Deleted;
            }
            // like exists and (IsLike == true)
            else
            {
                like.IsLike = !like.IsLike;
                _likeRepo.Update(like);
                var tags = _tagRepo.GetTagsByPictureId(picture.Id);
                foreach (var tag in tags)
                {
                    _tagRepo.TryDeleteAccountLikedTag(account, tag);
                }
                _pictureRepo.UpdatePicScore(picture);
                _likeRepo.Save();
                return LikeState.DisLiked;
            }
        }
        // does not exist
        else
        {
            _likeRepo.Insert(
                new Like()
                {
                    Liked = picture,
                    Liker = account,
                    IsLike = false
                });
            _pictureRepo.UpdatePicScore(picture);
            _likeRepo.Save();
            return LikeState.DisLiked;
        }
    }

}