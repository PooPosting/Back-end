#nullable enable
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PicturesAPI.Interfaces;

namespace PicturesAPI.Services;

public class AccountContextService : IAccountContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
    public string? GetAccountId => User is null ? null : User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
}