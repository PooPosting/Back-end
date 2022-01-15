using System;
using System.Security.Claims;
using PicturesAPI.Services;

namespace PicturesAPI.Interfaces;

public interface IPictureLikingService
{
    LikeOperationResult Like(Guid id);
    LikeOperationResult DisLike(Guid id);
}