using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PopularService: IPopularService
{
    private readonly IPictureRepo _pictureRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IMapper _mapper;

    public PopularService(
        IPictureRepo pictureRepo,
        IAccountRepo accountRepo,
        IMapper mapper)
    {
        _pictureRepo = pictureRepo;
        _accountRepo = accountRepo;
        _mapper = mapper;
    }

    public PopularContentDto GetPopularContent()
    {
        var mostVotedPics = _pictureRepo
            .GetPictures()
            .OrderByDescending(p => p.Likes.Count)
            .Take(5);
        var mostLikedPics = _pictureRepo
            .GetPictures()
            .OrderByDescending(p => p.Likes.Count(l => l.IsLike))
            .Take(5);
        var mostCommentedPics = _pictureRepo
            .GetPictures()
            .OrderByDescending(p => p.Comments.Count)
            .Take(5);

        var mostPostsAccs = _accountRepo
            .GetAccounts(DbInclude.Include)
            .OrderByDescending(a => a.Pictures.Count)
            .Take(5);
        var mostLikedAccs = _accountRepo
            .GetAccounts(DbInclude.Include)
            .OrderByDescending(CountPicLikes)
            .Take(5);

        var result = new PopularContentDto()
        {
            MostVotedPictures = _mapper.Map<ICollection<PictureDto>>(mostVotedPics),
            MostLikedPictures = _mapper.Map<ICollection<PictureDto>>(mostLikedPics),
            MostCommentedPictures = _mapper.Map<ICollection<PictureDto>>(mostCommentedPics),
            MostPostedAccounts = _mapper.Map<ICollection<AccountDto>>(mostPostsAccs),
            MostLikedAccounts = _mapper.Map<ICollection<AccountDto>>(mostLikedAccs),
        };
        return result;
    }

    private int CountPicLikes(Account account)
    {
        var result = 0;
        account.Pictures.ToList().ForEach(a => result += a.Likes.Count);
        return result;
    }
}