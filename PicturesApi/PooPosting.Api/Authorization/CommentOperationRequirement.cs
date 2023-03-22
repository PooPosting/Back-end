using Microsoft.AspNetCore.Authorization;
using PooPosting.Api.Enums;

namespace PooPosting.Api.Authorization;

public class CommentOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation CommentOperation { get; }

    public CommentOperationRequirement(ResourceOperation commentOperation)
    {
        CommentOperation = commentOperation;
    }
}