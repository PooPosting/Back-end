using Microsoft.AspNetCore.Authorization;
using PooPosting.Api.Enums;

namespace PooPosting.Api.Authorization;

public class AccountOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation AccountOperation { get; }

    public AccountOperationRequirement(ResourceOperation accountOperation)
    {
        AccountOperation = accountOperation;
    }
}