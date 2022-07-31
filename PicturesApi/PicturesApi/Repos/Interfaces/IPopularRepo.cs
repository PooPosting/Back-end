#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IPopularRepo
{
    Task<IEnumerable<Picture>> GetPicsByVoteCountAsync(int itemsToTake);
    Task<IEnumerable<Picture>> GetPicsByLikeCountAsync(int itemsToTake);
    Task<IEnumerable<Picture>> GetPicsByCommentCountAsync(int itemsToTake);
    Task<IEnumerable<Account>> GetAccsByPostCountAsync(int itemsToTake);
    Task<IEnumerable<Account>> GetAccsByPostLikesCountAsync(int itemsToTake);
}