using PicturesAPI.Enums;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    LikeState Like(int id);
    LikeState DisLike(int id);
}