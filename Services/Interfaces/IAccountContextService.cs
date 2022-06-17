#nullable enable
using System.Security.Claims;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountContextService
{
    ClaimsPrincipal User { get; }
    Guid GetAccountId { get; }
    int GetAccountRole { get; }
}