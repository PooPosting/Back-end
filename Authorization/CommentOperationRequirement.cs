using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Enums;

namespace PicturesAPI.Authorization;

public class CommentOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation CommentOperation { get; }

    public CommentOperationRequirement(ResourceOperation commentOperation)
    {
        CommentOperation = commentOperation;
    }
}