using PicturesAPI.Entities;

namespace PicturesAPI.Services.Helpers.Interfaces;

public interface ITagHelper
{
    Task<bool> TryInsertPictureTagJoinAsync(Picture picture, Tag tag);
    Task<bool> TryUpdatePictureTagsAsync(Picture picture, IEnumerable<string> tags);
}