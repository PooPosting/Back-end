#nullable enable
using System.Collections.ObjectModel;
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