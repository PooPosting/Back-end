#nullable enable
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Repos;

public class PictureRepo : IPictureRepo
{
    private readonly PictureDbContext _dbContext;

    public PictureRepo(
        PictureDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }

    public async Task<int> CountPicturesAsync(Expression<Func<Picture, bool>> predicate)
    {
        return await _dbContext.Pictures
            .Where(predicate)
            .CountAsync();
    }

    public async Task<Picture?> GetByIdAsync(int id)
    {
        return await _dbContext.Pictures
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .Include(p => p.Account)
            .ThenInclude(a => a.Role)
            .Include(p => p.PictureTags)
            .ThenInclude(j => j.Tag)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Account)
            .AsSplitQuery()
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Picture>> GetNotSeenByAccountIdAsync(int accountId, int itemsToTake)
    {
        return await _dbContext.Pictures
            .AsNoTracking()
            .OrderByDescending(p => (p.PictureTags
                .Select(t => t.Tag)
                .SelectMany(t => t.AccountLikedTags)
                .Select(alt => alt.AccountId == accountId).Count() + 1) * p.PopularityScore)
            .ThenByDescending(p => p.Id)
            .Where(p => !_dbContext.PicturesSeenByAccounts
                .Where(j => j.Account.Id == accountId)
                .Any(j => j.Picture.Id == p.Id && j.Account.Id == accountId))
            .Select(p => new Picture()
            {
                Id = p.Id,
                Url = p.Url,
                Name = p.Name,
                Description = p.Description,
                PictureAdded = p.PictureAdded,
                Account = new Account()
                {
                    Id = p.Account.Id,
                    Nickname = p.Account.Nickname,
                    ProfilePicUrl = p.Account.ProfilePicUrl
                },
                PictureTags = p.PictureTags.Select(t => new PictureTag()
                {
                    Id = t.Id,
                    Tag = new Tag()
                    {
                        Id = t.Tag.Id,
                        Value = t.Tag.Value,
                        AccountLikedTags = t.Tag.AccountLikedTags.Select(at => new AccountLikedTag()
                        {
                            Id = at.Id,
                            Account = new Account()
                            {
                                Id = at.Account.Id
                            },
                        }).AsEnumerable()
                    },
                }).AsEnumerable(),
                Likes = p.Likes.Select(l => new Like()
                {
                    Id = l.Id,
                    Account = new Account()
                    {
                        Id = l.Account.Id,
                        Nickname = l.Account.Nickname,
                    },
                    Picture = new Picture()
                    {
                        Id = l.Picture.Id
                    },
                    IsLike = l.IsLike
                }).AsEnumerable(),
                Comments = p.Comments.Select(c => new Comment()
                {
                    Id = c.Id,
                    CommentAdded = c.CommentAdded,
                    Account = new Account()
                    {
                        Id = c.Account.Id,
                        Nickname = c.Account.Nickname,
                    },
                    Picture = new Picture()
                    {
                        Id = c.Picture.Id
                    },
                    Text = c.Text
                }).AsEnumerable()
            })
            .Take(itemsToTake)
            .ToListAsync();
    }

    public async Task<IEnumerable<Picture>> SearchAllAsync(
        int itemsToSkip, int itemsToTake,
        Expression<Func<Picture, long>>? orderExp,
        Expression<Func<Picture, bool>>? filterExp)
    {
        var query = _dbContext.Pictures
            .AsNoTracking()
            .Select(p => new Picture()
            {
                Id = p.Id,
                Url = p.Url,
                Name = p.Name,
                Description = p.Description,
                PictureAdded = p.PictureAdded,
                PopularityScore = p.PopularityScore,
                Account = new Account()
                {
                    Id = p.Account.Id,
                    Nickname = p.Account.Nickname,
                    ProfilePicUrl = p.Account.ProfilePicUrl
                },
                PictureTags = p.PictureTags.Select(t => new PictureTag()
                {
                    Id = t.Id,
                    Tag = new Tag()
                    {
                        Id = t.Tag.Id,
                        Value = t.Tag.Value,
                        AccountLikedTags = t.Tag.AccountLikedTags.Select(at => new AccountLikedTag()
                        {
                            Id = at.Id,
                            Account = new Account()
                            {
                                Id = at.Account.Id
                            },
                        }).AsEnumerable()
                    },
                }).AsEnumerable(),
                Likes = p.Likes.Select(l => new Like()
                {
                    Id = l.Id,
                    Account = new Account()
                    {
                        Id = l.Account.Id,
                        Nickname = l.Account.Nickname,
                    },
                    Picture = new Picture()
                    {
                        Id = l.Picture.Id
                    },
                    IsLike = l.IsLike
                }).AsEnumerable(),
                Comments = p.Comments.Select(c => new Comment()
                {
                    Id = c.Id,
                    CommentAdded = c.CommentAdded,
                    Account = new Account()
                    {
                        Id = c.Account.Id,
                        Nickname = c.Account.Nickname,
                    },
                    Picture = new Picture()
                    {
                        Id = c.Picture.Id
                    },
                    Text = c.Text
                }).AsEnumerable()
            });

        if (orderExp is not null)
        {
            query = query.OrderByDescending(orderExp)
                .ThenByDescending(p => p.Id);
        }

        if (filterExp is not null)
        {
            query = query.Where(filterExp);
        }

        return await query
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .ToListAsync();
    }

    public async Task<Picture> InsertAsync(Picture picture)
    {
        await _dbContext.Pictures.AddAsync(picture);

        await _dbContext.SaveChangesAsync();
        return picture;
    }

    public async Task<Picture> UpdateAsync(Picture picture)
    {
        _dbContext.Pictures.Update(picture);
        await _dbContext.SaveChangesAsync();
        return picture;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var pic = _dbContext.Pictures.SingleOrDefault(p => p.Id == id);
        if (pic is null) return false;

        pic.IsDeleted = true;
        return await _dbContext.SaveChangesAsync() > 0;
    }
}