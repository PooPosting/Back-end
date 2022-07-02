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
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Tag>> GetTagsByPictureIdAsync(int pictureId)
    {
        return await _dbContext.PictureTags
            .Where(p => p.Picture.Id == pictureId)
            .Select(p => p.Tag)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Tag>> GetTagsByAccountIdAsync(int accountId)
    {
        return await _dbContext.AccountsLikedTags
            .Where(a => a.Account.Id == accountId)
            .Select(t => t.Tag)
            .ToArrayAsync();
    }

    public async Task<Tag> InsertAsync(Tag tag)
    {
        var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Value == tag.Value);
        if (existingTag is not null) return existingTag;

        await _dbContext.Tags.AddAsync(tag);
        await _dbContext.SaveChangesAsync();
        return tag;
    }
    public async Task<bool> TryInsertPictureTagJoinAsync(int pictureId, Tag tag)
    {
        if (!_dbContext.PictureTags
                .Any(j => (j.Picture.Id == pictureId) && (j.Tag.Id == tag.Id))
           )
        {
            if (!_dbContext.Tags.Any(t => t.Value == tag.Value))
            {
                await _dbContext.Tags.AddAsync(tag);
            }
            _dbContext.PictureTags.Add(new PictureTag()
            {
                PictureId = pictureId,
                TagId = tag.Id
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }

        return false;
    }
    public async Task<bool> TryInsertAccountLikedTagAsync(int accountId, Tag tag)
    {
        if (!_dbContext.AccountsLikedTags
            .Any(j => (j.Account.Id == accountId) && j.Tag.Id == tag.Id)
            )
        {
            if (!_dbContext.Tags.Any(t => t.Value == tag.Value))
            {
                await _dbContext.Tags.AddAsync(tag);
            }

            _dbContext.AccountsLikedTags.Add(new AccountLikedTags()
            {
                AccountId = accountId,
                TagId = tag.Id
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }

        return false;
    }
    public async Task<bool> TryDeleteAccountLikedTagAsync(int accountId, int tagId)
    {
        var accLikedTag = _dbContext.AccountsLikedTags
            .SingleOrDefault(j => (j.Account.Id == accountId) && (j.Tag.Id == tagId));

        if (accLikedTag is not null)
        {
            _dbContext.AccountsLikedTags.Remove(accLikedTag);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }
    public async Task<bool> TryDeletePictureTagJoinAsync(int pictureId, int tagId)
    {
        var picTag = _dbContext.PictureTags
            .SingleOrDefault(j => (j.Picture.Id == pictureId) && (j.Tag.Id == tagId));

        if (picTag is not null)
        {
            _dbContext.PictureTags.Remove(picTag);
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