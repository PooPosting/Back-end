#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ITagRepo
{
    Task<IEnumerable<Tag>> GetByPhraseAsync(string phrase);
    Task<IEnumerable<Tag>> GetTagsByPictureIdAsync(int pictureId);
    Task<IEnumerable<Tag>> GetTagsByAccountIdAsync(int accountId);
    Task<bool> TryInsertPictureTagJoinAsync(int pictureId, Tag tag);
    Task<bool> TryInsertAccountLikedTagAsync(int accountId, Tag tag);
    Task<bool> TryDeleteAccountLikedTagAsync(int accountId, int tagId);
    Task<bool> TryDeletePictureTagJoinAsync(int pictureId, int tagId);
    Task<Tag> InsertAsync(Tag tag);
    Task<Tag> UpdateAsync(Tag tag);
    Task<Tag> DeleteAsync(Tag tag);
}