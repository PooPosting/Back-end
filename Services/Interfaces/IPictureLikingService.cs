using PicturesAPI.Enums;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    Task<LikeState> Like(int id);
    Task<LikeState> DisLike(int id);
}