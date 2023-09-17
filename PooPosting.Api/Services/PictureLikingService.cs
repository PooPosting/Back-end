using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;
using PooPosting.Api.Entities.Joins;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Helpers.Interfaces;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Models.Dtos;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Services;

public class PictureLikingService : IPictureLikingService
{
    private readonly IMapper _mapper;
    private readonly IAccountContextService _accountContextService;
    private readonly PictureDbContext _dbContext;

    public PictureLikingService(
        IMapper mapper,
        IAccountContextService accountContextService,
        PictureDbContext dbContext)
    {
        _mapper = mapper;
        _accountContextService = accountContextService;
        _dbContext = dbContext;
    }
    
    public async Task<PictureDto> Like(int pictureId)
    {
        var picture = await _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == pictureId);

        if (picture == null) throw new NotFoundException();
        var accountId = _accountContextService.GetAccountId();
        var pictureResult = await LikeAsync(picture, accountId);
        var mappedResult = _mapper.Map<PictureDto>(pictureResult);
        return mappedResult;
    }

    public async Task<PictureDto> DisLike(int pictureId)
    {
        var picture = await _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == pictureId);
            
        if (picture == null) throw new NotFoundException();
        var accountId = _accountContextService.GetAccountId();
        var pictureResult = await DislikeAsync(picture, accountId);
        var mappedResult = _mapper.Map<PictureDto>(pictureResult);
        return mappedResult;
    }
    
    private async Task<Picture> LikeAsync(Picture picture, int accountId)
    {
        var like = picture.Likes.SingleOrDefault(l => l.AccountId == accountId);

        if (like == null)
        {
            // User hasn't liked this picture before
            picture.Likes.Add(new Like
            {
                AccountId = accountId,
                PictureId = picture.Id,
                IsLike = true,
            });

            // Update LikedTags
            foreach (var pictureTag in picture.PictureTags)
            {
                pictureTag.Tag.AccountLikedTags.Add(new AccountLikedTag
                {
                    AccountId = accountId,
                    TagId = pictureTag.TagId,
                });
            }
        }
        else
        {
            if (like.IsLike)
            {
                // User previously liked the picture; now unliking it
                picture.Likes.Remove(like);

                // Remove LikedTags entries
                foreach (var pictureTag in picture.PictureTags)
                {
                    var likedTag = pictureTag.Tag.AccountLikedTags
                        .SingleOrDefault(alt => alt.AccountId == accountId && alt.TagId == pictureTag.TagId);

                    if (likedTag != null)
                    {
                        pictureTag.Tag.AccountLikedTags.Remove(likedTag);
                    }
                }
            }
            else
            {
                // User previously unliked the picture; now liking it
                like.IsLike = true;

                // Update LikedTags
                foreach (var pictureTag in picture.PictureTags)
                {
                    pictureTag.Tag.AccountLikedTags.Add(new AccountLikedTag
                    {
                        AccountId = accountId,
                        TagId = pictureTag.TagId,
                    });
                }
            }
        }

        // Update PopularityScore
        picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);

        await _dbContext.SaveChangesAsync();
        return picture;
    }

    private async Task<Picture> DislikeAsync(Picture picture, int accountId)
    {
        var like = picture.Likes.SingleOrDefault(l => l.AccountId == accountId);

        if (like == null)
        {
            // User hasn't disliked this picture before
            picture.Likes.Add(new Like
            {
                AccountId = accountId,
                PictureId = picture.Id,
                IsLike = false,
            });

            // No need to update LikedTags in this case
        }
        else
        {
            if (!like.IsLike)
            {
                // User previously disliked the picture; now removing the dislike
                picture.Likes.Remove(like);

                // No need to update LikedTags in this case
            }
            else
            {
                like.IsLike = false;

                // Remove LikedTags entries
                foreach (var pictureTag in picture.PictureTags)
                {
                    var likedTag = pictureTag.Tag.AccountLikedTags
                        .SingleOrDefault(alt => alt.AccountId == accountId && alt.TagId == pictureTag.TagId);

                    if (likedTag != null)
                    {
                        pictureTag.Tag.AccountLikedTags.Remove(likedTag);
                    }
                }
            }
        }

        // Update PopularityScore
        picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);

        await _dbContext.SaveChangesAsync();
        return picture;
    }
}