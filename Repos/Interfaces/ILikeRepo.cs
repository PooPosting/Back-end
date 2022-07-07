#nullable enable
using PicturesAPI.Entities;
namespace PicturesAPI.Repos.Interfaces;

public interface ILikeRepo
{
    Task<Like?> GetByIdAsync(int id);
    Task<IEnumerable<Like>> GetByLikerIdAsync(int id);
    Task<IEnumerable<Like>> GetByLikedIdAsync(int id);
    Task<Picture> LikeAsync(int pictureId, int accountId);
    Task<Picture> DislikeAsync(int pictureId, int accountId);
}