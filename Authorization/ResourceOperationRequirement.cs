using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Enums;

namespace PicturesAPI.Authorization;



public class ResourceOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation ResourceOperation { get; }

    public ResourceOperationRequirement(ResourceOperation resourceOperation)
    {
        ResourceOperation = resourceOperation;
    }
}