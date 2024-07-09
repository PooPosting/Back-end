#nullable enable
using System.Security.Claims;
using PooPosting.Data.DbContext.Entities;

namespace PooPosting.Service.Services.Interfaces;

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