using Microsoft.AspNetCore.Authorization;
using PooPosting.Data.Enums;

namespace PooPosting.Service.Authorization;

public class PictureOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation PictureOperation { get; }

    public PictureOperationRequirement(ResourceOperation pictureOperation)
    {
        PictureOperation = pictureOperation;
    }
}