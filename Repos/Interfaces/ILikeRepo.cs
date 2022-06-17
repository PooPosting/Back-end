using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ILikeRepo
{
    Task<List<Like>> GetByLikerId(Guid id);
    Task<List<Like>> GetByLikedId(Guid id);
    Task<Like> GetByLikerIdAndLikedId(Guid accountId, Guid pictureId);
    Task Insert(Like like);
    Task Delete(Like like);
    Task Update(Like like);
    Task Save();
}