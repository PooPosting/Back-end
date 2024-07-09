using Microsoft.AspNetCore.Authorization;
using PooPosting.Data.Enums;

namespace PooPosting.Service.Authorization;

public class AccountOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation AccountOperation { get; }

    public AccountOperationRequirement(ResourceOperation accountOperation)
    {
        AccountOperation = accountOperation;
    }
}