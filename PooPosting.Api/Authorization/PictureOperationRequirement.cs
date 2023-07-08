using Microsoft.AspNetCore.Authorization;
using PooPosting.Api.Enums;

namespace PooPosting.Api.Authorization;

public class PictureOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation PictureOperation { get; }

    public PictureOperationRequirement(ResourceOperation pictureOperation)
    {
        PictureOperation = pictureOperation;
    }
}