using PicturesAPI.Models;
using PicturesAPI.Models.Dtos.Like;
using PicturesAPI.Models.Queries;

namespace PicturesAPI.Services.Interfaces;

public interface ILikeService
{
    Task<PagedResult<LikeDto>> GetLikesByPictureId(Query query, int picId);
}