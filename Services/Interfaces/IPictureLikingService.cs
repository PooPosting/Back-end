using System;
using System.Security.Claims;
using PicturesAPI.Enums;
using PicturesAPI.Services;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureLikingService
{
    LikeOperationResult Like(Guid id);
    LikeOperationResult DisLike(Guid id);
}