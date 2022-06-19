#nullable enable
using System.Security.Claims;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class AccountContextService : IAccountContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PictureDbContext _dbContext;

    public AccountContextService(
        IHttpContextAccessor httpContextAccessor,
        PictureDbContext dbContext
        )
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext!.User;

    public int GetAccountId()
    {
        ValidateJwt();
        return int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
    }

    public int? TryGetAccountId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return idClaim is not null ? int.Parse(idClaim) : null;
    }

    public int GetAccountRole()
    {
        ValidateJwt();
        return int.Parse(User.FindFirst(c => c.Type == ClaimTypes.Role)!.Value);
    }


    private void ValidateJwt()
    {
        // test
        var exp = User.FindAll(c => true);

        // foreach (var item in exp)
        // {
        //     Console.WriteLine($"{item.Type} - {item.Value}");
        // }

        var role = User.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;
        var id = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        if (
            (id is null) || (role is null) || (exp is null) ||
            (!_dbContext.Accounts.Any(a => a.Id == int.Parse(id)))
        )
        {
            throw new UnauthorizedException("please log in");
        }

        if (
            (User.FindFirst(c => c.Type == "exp") is null) ||
            (TimeSpan.FromSeconds(int.Parse(User.FindFirst(c => c.Type == "exp")!.Value)) < TimeSpan.FromMinutes(0))
        )
        {
            throw new ExpiredJwtException("jwt token has expired.");
        }

    }
}