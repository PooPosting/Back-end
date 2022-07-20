using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Services.Helpers.Interfaces;

namespace PicturesAPI.Services.Helpers;

public class TagHelper : ITagHelper
{
    private readonly PictureDbContext _dbContext;

    public TagHelper(
        PictureDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }

    public async Task<bool> TryInsertPictureTagJoinAsync(Picture picture, Tag tag)
    {
        if (!_dbContext.Tags.Any(t => t.Value == tag.Value))
        {
            await _dbContext.Tags.AddAsync(tag);
        }

        if (!_dbContext.PictureTags
                .Any(pt => pt.PictureId == picture.Id && pt.TagId == tag.Id))
        {
            await _dbContext.PictureTags.AddAsync(new PictureTag()
            {
                PictureId = picture.Id,
                TagId = tag.Id
            });
        }

        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> TryUpdatePictureTagsAsync(Picture picture, IEnumerable<string> tags)
    {
        var tagEntities = tags
            .Distinct()
            .ToList()
            .Select(tag => new Tag() { Value = tag });

        var tagsToAdd = new Collection<Tag>();

        foreach (var tagEntity in tagEntities)
        {
            var tag = await _dbContext.Tags.SingleOrDefaultAsync(t => t.Value == tagEntity.Value);

            if (tag is null)
            {
                await _dbContext.Tags.AddAsync(tagEntity);
                tagsToAdd.Add(tagEntity);
            }
            else
            {
                tagsToAdd.Add(tag);
            }
        }
        var pictureTags = tagsToAdd
            .Select(tagToAdd => new PictureTag() { Tag = tagToAdd, Picture = picture });

        picture.PictureTags = pictureTags.ToList();

        _dbContext.Update(picture);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}