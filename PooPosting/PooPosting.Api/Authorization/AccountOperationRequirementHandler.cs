using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Enums;

namespace PooPosting.Api.Authorization;

public class AccountOperationRequirementHandler : AuthorizationHandler<AccountOperationRequirement, Account>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccountOperationRequirement requirement,
        Account account)
    {
        if (requirement.AccountOperation == ResourceOperation.Create ||
            requirement.AccountOperation == ResourceOperation.Read)
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