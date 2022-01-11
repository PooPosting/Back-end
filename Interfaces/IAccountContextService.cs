using System.Security.Claims;

namespace PicturesAPI.Interfaces;

public interface IAccountContextService
{
    ClaimsPrincipal User { get; }
    string? GetAccountId { get; }
}