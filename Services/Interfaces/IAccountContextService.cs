#nullable enable
using System.Security.Claims;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountContextService
{
    ClaimsPrincipal User { get; }
    int? TryGetAccountId();
    int GetAccountId();
    int GetAccountRole();
}