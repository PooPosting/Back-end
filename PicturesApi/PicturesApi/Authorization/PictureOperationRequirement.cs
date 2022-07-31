using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Enums;

namespace PicturesAPI.Authorization;

public class PictureOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation PictureOperation { get; }

    public PictureOperationRequirement(ResourceOperation pictureOperation)
    {
        PictureOperation = pictureOperation;
    }
}