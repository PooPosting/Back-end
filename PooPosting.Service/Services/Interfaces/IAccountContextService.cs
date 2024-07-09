#nullable enable
using System.Security.Claims;
using PooPosting.Domain.DbContext.Entities;

namespace PooPosting.Application.Services.Interfaces;

public interface IAccountContextService
{
    string GetEncodedAccountId();
    ClaimsPrincipal User { get; }
    int? TryGetAccountId();
    int GetAccountId();
    Task<Account> GetTrackedAccountAsync();
    Task<Account> GetAccountAsync();
    int GetAccountRole();
}