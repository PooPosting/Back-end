using PicturesAPI.Enums;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    LikeOperationResult Like(Guid id);
    LikeOperationResult DisLike(Guid id);
}