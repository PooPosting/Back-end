#nullable enable
using System.Linq.Expressions;
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IPictureRepo
{
    Task<int> CountPicturesAsync(Expression<Func<Picture, bool>> predicate);
    Task<int> CountPicturesAsync();
    Task<Picture?> GetByIdAsync(int id);
    Task<IEnumerable<Picture>> GetLikedPicturesByAccountIdAsync(int accountId, int itemsToSkip, int itemsToTake);
    Task<IEnumerable<Picture>> GetPostedPicturesByAccountIdAsync(int accountId, int itemsToSkip, int itemsToTake);
    Task<IEnumerable<Picture>> GetNotSeenByAccountIdAsync(int accountId, int itemsToTake);
    Task<IEnumerable<Picture>> SearchAllAsync(
        int itemsToSkip,
        int itemsToTake,
        Expression<Func<Picture, long>>? orderExp,
        Expression<Func<Picture, bool>>? filterExp);
    Task<Picture> InsertAsync(Picture picture);
    Task<Picture> UpdateAsync(Picture picture);
}