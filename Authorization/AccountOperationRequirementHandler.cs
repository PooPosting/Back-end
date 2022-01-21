using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PicturesAPI.Entities;
using PicturesAPI.Enums;

namespace PicturesAPI.Authorization;

public class AccountOperationRequirementHandler : AuthorizationHandler<AccountOperationRequirement, Account>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccountOperationRequirement requirement,
        Account account)
    {
        if (requirement.AccountOperation == AccountOperation.Create ||
            requirement.AccountOperation == AccountOperation.Read)
        {
            context.Succeed(requirement);
        }
        
        var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;

        
        if (account.Id.ToString() == userId)
        {
            context.Succeed(requirement);
        }
        if (userRole is "3" or "Administrator")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
        
    }
}