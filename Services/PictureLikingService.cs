using System.Security.Claims;
using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
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
    private readonly IAccountRepo _accountRepo;
    private readonly IAccountContextService _accountContextService;

    public PictureLikingService(
        ILikeRepo likeRepo,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        IMapper mapper,
        IAccountContextService accountContextService)
    {
        _pictureRepo = pictureRepo;
        _mapper = mapper;
        _likeRepo = likeRepo;
        _accountRepo = accountRepo;
        _accountContextService = accountContextService;
    }
    
    public PictureDto Like(int id)
    {
        var picture = _pictureRepo.GetById(id);
        var accountId = _accountContextService.GetAccountId();

        if (picture is null) throw new NotFoundException("picture not found");

        // set up like/dislike logic (database liked tags insertion/drop)
        var account = _accountRepo.GetById(accountId);
        var like = _likeRepo.GetByLikerIdAndLikedId(account.Id, picture.Id);
        
        if (like is not null)
        {
            // like exists and (IsLike == true)
            if (like.IsLike)
            {
                _likeRepo.DeleteById(like.Id);
            }
            // like exists and (IsLike == false)
            else
            {
                like.IsLike = !like.IsLike;
                _likeRepo.Update(like);
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
        }

        _likeRepo.Save();
        return _mapper.Map<PictureDto>(picture);
    }

    public PictureDto DisLike(int id)
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
            }
            // like exists and (IsLike == true)
            else
            {
                like.IsLike = !like.IsLike;
                _likeRepo.Update(like);
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
        }

        _likeRepo.Save();
        return _mapper.Map<PictureDto>(picture);
    }

}