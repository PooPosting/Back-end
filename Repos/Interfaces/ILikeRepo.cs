using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ILikeRepo
{
    List<Like> GetLikesByLiker(Account liker);
    List<Like> GetLikesByLiker(Guid id);
    List<Like> GetLikesByLiked(Picture picture);
    List<Like> GetLikesByLiked(Guid id);
    int RemoveLikes(List<Like> likes);
    Like GetLikeByLikerAndLiked(Account account, Picture picture);
    void AddLike(Like like);
    void RemoveLike(Like like);
    void ChangeLike(Like like);

}