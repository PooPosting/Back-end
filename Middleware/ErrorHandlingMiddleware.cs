using HashidsNet;
using PicturesAPI.Exceptions;

namespace PicturesAPI.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
        
    public ErrorHandlingMiddleware(
        ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }

        catch (BadRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(badRequestException.Message);
        }

        catch (InvalidAuthTokenException invalidAuthTokenException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(invalidAuthTokenException.Message);
        }
        catch (UnauthorizedException unauthorizedException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(unauthorizedException.Message);
        }
        catch (ForbidException forbidException)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(forbidException.Message);
        }
        catch (RestrictedException restrictedException)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(restrictedException.Message);
        }

        catch (NoResultException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("resource not found");
        }
        catch (NotFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("resource not found");
        }

        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
        
}