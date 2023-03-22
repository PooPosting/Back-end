using System.Security.Claims;

namespace PooPosting.Api.Middleware;

public class UserDataMiddleware: IMiddleware
{
    private readonly ILogger<UserDataMiddleware> _logger;

    public UserDataMiddleware(
        ILogger<UserDataMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next.Invoke(context);

        var userName = context.User.FindFirst(c => c.Type == ClaimTypes.Name);
        var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role);
        var log = $"## Client IP: '{context.Connection.RemoteIpAddress}'";
        
        if (userName is not null && userRole is not null)
        {
            log += $" ## Client Nickname: {userName.Value}, RoleID: {userRole.Value}";
        }
        log += $" ## Request: '{context.Request.Method}' at '{context.Request.Path}', with status code: '{context.Response.StatusCode}'. ##";

        switch (context.Request.Method)
        {
            case ("GET"):
                _logger.LogInformation(log);
                break;
            
            default:
                _logger.LogWarning(log);
                break;
        }
    }
}