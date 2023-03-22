using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;
using PooPosting.Api.Entities.Joins;
using PooPosting.Api.Services.Helpers.Interfaces;

namespace PooPosting.Api.Services.Helpers;

public class LikeHelper: ILikeHelper
{
    private readonly PictureDbContext _dbContext;

    public LikeHelper(
        PictureDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }

    public async Task<Picture> LikeAsync(
        int picId,
        int accId
        )
    {
        var account = _dbContext.Accounts
            .Include(a => a.LikedTags)
            .SingleOrDefault(a => a.Id == accId)!;

        var picture = _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .SingleOrDefault(p => p.Id == picId)!;

        var like = picture.Likes.SingleOrDefault(l => l.AccountId == accId);

        List<AccountLikedTag> accLikedTags = account.LikedTags.ToList();
        List<Like> picLikes = picture.Likes.ToList();

        if (like is null)
        {
            accLikedTags.AddRange(picture.PictureTags
                .Select(pictureTag => new AccountLikedTag()
                {
                    AccountId = accId, TagId = pictureTag.TagId
                }));
            picLikes.Add(new Like()
            {
                AccountId = accId,
                PictureId = picId,
                IsLike = true,
            });
            picture.Likes = picLikes;
            account.LikedTags = accLikedTags;
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            if (like.IsLike)
            {
                foreach (var pictureTag in picture.PictureTags)
                {
                    accLikedTags = accLikedTags
                        .Where(alt => alt.TagId != pictureTag.TagId)
                        .ToList();
                }
                picLikes.Remove(like);
            }
            else
            {
                like.IsLike = !like.IsLike;
                accLikedTags.AddRange(picture.PictureTags
                    .Select(pictureTag => new AccountLikedTag()
                    {
                        AccountId = accId, TagId = pictureTag.TagId
                    }));
            }
        }
        picture.Likes = picLikes;
        account.LikedTags = accLikedTags;
        picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
        await _dbContext.SaveChangesAsync();
        return picture;
    }

    public async Task<Picture> DislikeAsync(
        int picId,
        int accId
        )
    {
        var account = _dbContext.Accounts
            .Include(a => a.LikedTags)
            .SingleOrDefault(a => a.Id == accId)!;

        var picture = _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .SingleOrDefault(p => p.Id == picId)!;

        var like = picture.Likes.SingleOrDefault(l => l.AccountId == accId);

        List<AccountLikedTag> accLikedTags = account.LikedTags.ToList();
        List<Like> picLikes = picture.Likes.ToList();

        if (like is null)
        {
            picLikes.Add(new Like()
            {
                AccountId = accId,
                PictureId = picId,
                IsLike = false,
            });
            picture.Likes = picLikes;
            account.LikedTags = accLikedTags;
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
            await _dbContext.SaveChangesAsync();
            return picture;
        }
        else
        {
            if (like.IsLike)
            {
                like.IsLike = !like.IsLike;
                foreach (var pictureTag in picture.PictureTags)
                {
                    accLikedTags = accLikedTags
                        .Where(alt => alt.TagId != pictureTag.TagId)
                        .ToList();
                }
            }
            else
            {
                picLikes.Remove(like);
            }
            picture.Likes = picLikes;
            account.LikedTags = accLikedTags;
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
            await _dbContext.SaveChangesAsync();
            return picture;
        }
    }
}