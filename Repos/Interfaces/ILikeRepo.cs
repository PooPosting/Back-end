using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ILikeRepo
{
    List<Like> GetByLikerId(int id);
    List<Like> GetByLikedId(int id);
    Like GetByLikerIdAndLikedId(int accountId, int pictureId);
    void Insert(Like like);
    void DeleteById(int id);
    void Update(Like like);
    bool Save();
}