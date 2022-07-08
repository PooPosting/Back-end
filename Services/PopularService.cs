using AutoMapper;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PopularService : IPopularService
{
    private readonly IPopularRepo _popularRepo;
    private readonly IMapper _mapper;

    public PopularService(
        IPopularRepo popularRepo,
        IMapper mapper)
    {
        _popularRepo = popularRepo;
        _mapper = mapper;
    }

    public async Task<PopularContentDto> Get()
    {
        var mostVotedPics = await _popularRepo.GetPicsByVoteCountAsync(5);
        var mostLikedPics = await _popularRepo.GetPicsByLikeCountAsync(5);
        var mostCommentedPics = await _popularRepo.GetPicsByCommentCountAsync(5);

        var mostPostsAccs = await _popularRepo.GetAccsByPostCountAsync(5);
        var mostLikedAccs = await _popularRepo.GetAccsByPostLikesCountAsync(5);

        var result = new PopularContentDto()
        {
            MostVotedPictures = _mapper.Map<IEnumerable<PictureDto>>(mostVotedPics),
            MostLikedPictures = _mapper.Map<IEnumerable<PictureDto>>(mostLikedPics),
            MostCommentedPictures = _mapper.Map<IEnumerable<PictureDto>>(mostCommentedPics),
            MostPostedAccounts = _mapper.Map<IEnumerable<AccountDto>>(mostPostsAccs),
            MostLikedAccounts = _mapper.Map<IEnumerable<AccountDto>>(mostLikedAccs),
        };
        return result;
    }
}