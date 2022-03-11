#nullable enable
using System.Security.Claims;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountContextService
{
    ClaimsPrincipal User { get; }
    string? GetAccountId { get; }
    string? GetAccountRole { get; }
}