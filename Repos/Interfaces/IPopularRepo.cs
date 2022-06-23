using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface IPopularRepo
{
    IEnumerable<Picture> GetPicsByVoteCount(int itemsToTake);
    IEnumerable<Picture> GetPicsByLikeCount(int itemsToTake);
    IEnumerable<Picture> GetPicsByCommentCount(int itemsToTake);
    IEnumerable<Account> GetAccsByPostCount(int itemsToTake);
    IEnumerable<Account> GetAccsByPostLikesCount(int itemsToTake);
}