using PicturesAPI.Models.Dtos.Like;

namespace PicturesAPI.Services.Interfaces;

public interface ILikeService
{
    Task<IEnumerable<LikeDto>> GetLikesByPicture(int id);
}