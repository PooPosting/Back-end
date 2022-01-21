using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Enums;

namespace PicturesAPI.Authorization;

public class AccountOperationRequirement : IAuthorizationRequirement
{
    public AccountOperation AccountOperation { get; }

    public AccountOperationRequirement(AccountOperation accountOperation)
    {
        AccountOperation = accountOperation;
    }
}