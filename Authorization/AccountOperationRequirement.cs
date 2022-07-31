using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Enums;

namespace PicturesAPI.Authorization;

public class AccountOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation AccountOperation { get; }

    public AccountOperationRequirement(ResourceOperation accountOperation)
    {
        AccountOperation = accountOperation;
    }
}