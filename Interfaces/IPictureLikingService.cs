using System;
using System.Security.Claims;
using PicturesAPI.Services;

namespace PicturesAPI.Interfaces;

public interface IPictureLikingService
{
    LikeOperationResult LikePicture(Guid id);
    LikeOperationResult DisLikePicture(Guid id);
}