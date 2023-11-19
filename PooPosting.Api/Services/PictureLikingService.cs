using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;
using PooPosting.Api.Entities.Joins;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Mappers;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Services.Interfaces;
using PooPosting.Api.Services.Helpers;

namespace PooPosting.Api.Services;

public class PictureLikingService : IPictureLikingService
{
    private readonly IAccountContextService _accountContextService;
    private readonly PictureDbContext _dbContext;

    public PictureLikingService(
        IAccountContextService accountContextService,
        PictureDbContext dbContext)
    {
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
            .ThenInclude(t => t.AccountLikedTags)
            .FirstOrDefaultAsync(p => p.Id == pictureId);

        if (picture == null) throw new NotFoundException();
        var accountId = _accountContextService.GetAccountId();
        var pictureResult = await LikeAsync(picture, accountId);

        return await _dbContext.Pictures
            .Where(p => p.Id == pictureResult.Id)
            .ProjectToDto(accountId)
            .FirstOrDefaultAsync();
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
        return pictureResult.MapToDto(accountId);
    }
    
    private async Task<Picture> LikeAsync(Picture picture, int accountId)
    {
        var like = picture.Likes.FirstOrDefault(l => l.AccountId == accountId);

        if (like == null)
        {
            // User hasn't liked this picture before
            picture.Likes.Add(new Like
            {
                AccountId = accountId,
                PictureId = picture.Id,
                Liked = DateTime.Now
            });

            // Update LikedTags
            foreach (var pictureTag in picture.PictureTags)
            {
                var entryExists = pictureTag.Tag.AccountLikedTags.Any(alt =>
                    alt.AccountId == accountId && alt.TagId == pictureTag.TagId);

                if (entryExists) continue;

                pictureTag.Tag.AccountLikedTags.Add(new AccountLikedTag
                {
                    AccountId = accountId,
                    TagId = pictureTag.TagId,
                });
            }
        }
        else
        {
            picture.Likes.Remove(like);
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
                Liked = DateTime.Now
            });

            // No need to update LikedTags in this case
        }
        else
        {
            picture.Likes.Remove(like);
        }

        // Update PopularityScore
        picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);

        await _dbContext.SaveChangesAsync();
        return picture;
    }
}