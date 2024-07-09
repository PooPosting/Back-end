using Microsoft.AspNetCore.Authorization;
using PooPosting.Data.Enums;

namespace PooPosting.Service.Authorization;

public class CommentOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation CommentOperation { get; }

    public CommentOperationRequirement(ResourceOperation commentOperation)
    {
        CommentOperation = commentOperation;
    }
}