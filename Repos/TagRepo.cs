#nullable enable
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class TagRepo : ITagRepo
{
    private readonly PictureDbContext _dbContext;

    public TagRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Tag>> GetByPhraseAsync(string phrase)
    {
        return await _dbContext.Tags
            .Where(t => t.Value.Contains(phrase))
            .Take(5)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> GetTagsByPictureIdAsync(int pictureId)
    {
        return await _dbContext.PictureTagJoins
            .Where(p => p.PictureId == pictureId)
            .Select(p => p.Tag)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> GetTagsByAccountIdAsync(int accountId)
    {
        return await _dbContext.AccountLikedTagJoins
            .Where(a => a.AccountId == accountId)
            .Select(t => t.Tag)
            .ToListAsync();
    }

    public async Task<Tag> InsertAsync(Tag tag)
    {
        var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Value == tag.Value);
        if (existingTag is not null) return existingTag;

        await _dbContext.Tags.AddAsync(tag);
        await _dbContext.SaveChangesAsync();
        return tag;
    }

    public async Task<bool> TryInsertPictureTagJoinAsync(Picture picture, Tag tag)
    {
        if (!_dbContext.PictureTagJoins
                .Any(j => (j.Picture == picture) && (j.Tag == tag))
           )
        {
            if (!_dbContext.Tags.Any(t => t.Value == tag.Value))
            {
                await _dbContext.Tags.AddAsync(tag);
            }
            _dbContext.PictureTagJoins.Add(new PictureTagJoin()
            {
                Picture = picture,
                Tag = tag
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<bool> TryInsertAccountLikedTagAsync(Account account, Tag tag)
    {
        if (!_dbContext.AccountLikedTagJoins
            .Any(j => (j.Account == account) && j.Tag == tag)
            )
        {
            if (!_dbContext.Tags.Any(t => t.Value == tag.Value))
            {
                await _dbContext.Tags.AddAsync(tag);
            }

            _dbContext.AccountLikedTagJoins.Add(new AccountLikedTagsJoin()
            {
                Account = account,
                Tag = tag
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<bool> TryDeleteAccountLikedTagAsync(Account account, Tag tag)
    {
        var accLikedTag = _dbContext.AccountLikedTagJoins
            .SingleOrDefault(j => (j.Account == account) && (j.Tag == tag));

        if (accLikedTag is not null)
        {
            _dbContext.AccountLikedTagJoins.Remove(accLikedTag);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }
    public async Task<bool> TryDeletePictureTagJoinAsync(Picture picture, Tag tag)
    {
        var picTag = _dbContext.PictureTagJoins
            .SingleOrDefault(j => (j.Picture == picture) && (j.Tag == tag));

        if (picTag is not null)
        {
            _dbContext.PictureTagJoins.Remove(picTag);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<Tag> UpdateAsync(Tag tag)
    {
        _dbContext.Tags.Update(tag);
        await _dbContext.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> DeleteAsync(Tag tag)
    {
        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync();
        return tag;
    }

}