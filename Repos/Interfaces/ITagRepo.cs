#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ITagRepo
{
    Task<IEnumerable<Tag>> GetByPhraseAsync(string phrase);
    Task<IEnumerable<Tag>> GetTagsByPictureIdAsync(int pictureId);
    Task<IEnumerable<Tag>> GetTagsByAccountIdAsync(int accountId);
    Task<bool> TryInsertPictureTagJoinAsync(Picture picture, Tag tag);
    Task<bool> TryInsertAccountLikedTagAsync(Account account, Tag tag);
    Task<bool> TryDeleteAccountLikedTagAsync(Account account, Tag tag);
    Task<Tag> InsertAsync(Tag tag);
    Task<Tag> UpdateAsync(Tag tag);
    Task<Tag> DeleteAsync(Tag tag);
}