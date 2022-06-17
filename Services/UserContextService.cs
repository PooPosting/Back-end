#nullable enable
using System.Security.Claims;
using PicturesAPI.Exceptions;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class AccountContextService : IAccountContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext!.User;
    public Guid GetAccountId => Guid.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
    public int GetAccountRole => int.Parse(User.FindFirst(c => c.Type == ClaimTypes.Role)!.Value);
}