#nullable enable
using PicturesAPI.Entities;
namespace PicturesAPI.Repos.Interfaces;

public interface ILikeRepo
{
    Task<Like?> GetByIdAsync(int id);
    Task<Like?> GetByLikerIdAndLikedIdAsync(int accountId, int pictureId);
    Task<IEnumerable<Like>> GetByLikerIdAsync(int id);
    Task<IEnumerable<Like>> GetByLikedIdAsync(int id);
    Task<Like> InsertAsync(Like like);
    Task<Like> DeleteByIdAsync(int id);
    Task<Like> UpdateAsync(Like like);
}