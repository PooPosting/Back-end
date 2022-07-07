#nullable enable
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Repos;

public class LikeRepo : ILikeRepo
{
    private readonly PictureDbContext _dbContext;

    public LikeRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Like?> GetByIdAsync(int id)
    {
        return await _dbContext.Likes.SingleOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Like?> GetByLikerIdAndLikedIdAsync(int accountId, int pictureId)
    {
        return await _dbContext.Likes
            .Include(l => l.Account)
            .Include(l => l.Picture)
            .FirstOrDefaultAsync(l => l.AccountId == accountId && l.PictureId == pictureId);
    }

    public async Task<IEnumerable<Like>> GetByLikerIdAsync(int id)
    {
        return await _dbContext.Likes.Where(l => l.AccountId == id)
            .Include(a => a.Picture)
            .Include(a => a.Account)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Like>> GetByLikedIdAsync(int id)
    {
        return await _dbContext.Likes.Where(l => l.PictureId == id)
            .Include(a => a.Picture)
            .Include(a => a.Account)
            .ToArrayAsync();
    }

    public async Task<Picture> LikeAsync(int pictureId, int accountId)
    {
        var account = _dbContext.Accounts
            .Include(a => a.LikedTags)
            .SingleOrDefault(a => a.Id == accountId)!;

        var picture = _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .SingleOrDefault(p => p.Id == pictureId)!;

        var like = picture.Likes.SingleOrDefault(l => l.AccountId == accountId);

        List<AccountLikedTag> accLikedTags = account.LikedTags.ToList();
        List<Like> picLikes = picture.Likes.ToList();

        if (like is null)
        {
            accLikedTags.AddRange(picture.PictureTags
                .Select(pictureTag => new AccountLikedTag()
                {
                    AccountId = accountId, TagId = pictureTag.TagId
                }));
            picLikes.Add(new Like()
            {
                AccountId = accountId,
                PictureId = pictureId,
                IsLike = true,
            });
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
            account.LikedTags = accLikedTags;
            picture.Likes = picLikes;
            await _dbContext.SaveChangesAsync();
            return picture;
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
                        AccountId = accountId, TagId = pictureTag.TagId
                    }));
            }
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
            account.LikedTags = accLikedTags;
            picture.Likes = picLikes;
            await _dbContext.SaveChangesAsync();
            return picture;
        }
    }

    public async Task<Picture> DislikeAsync(int pictureId, int accountId)
    {
        var account = _dbContext.Accounts
            .Include(a => a.LikedTags)
            .SingleOrDefault(a => a.Id == accountId)!;

        var picture = _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .SingleOrDefault(p => p.Id == pictureId)!;

        var like = picture.Likes.SingleOrDefault(l => l.AccountId == accountId);

        List<AccountLikedTag> accLikedTags = account.LikedTags.ToList();
        List<Like> picLikes = picture.Likes.ToList();

        if (like is null)
        {
            picLikes.Add(new Like()
            {
                AccountId = accountId,
                PictureId = pictureId,
                IsLike = false,
            });
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
            account.LikedTags = accLikedTags;
            picture.Likes = picLikes;
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
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
            account.LikedTags = accLikedTags;
            picture.Likes = picLikes;
            await _dbContext.SaveChangesAsync();
            return picture;
        }
    }

}