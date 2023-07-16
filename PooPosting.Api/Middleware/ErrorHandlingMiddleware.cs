using System.Globalization;
using FluentValidation;
using HashidsNet;
using PooPosting.Api.Exceptions;

namespace PooPosting.Api.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
        
    public ErrorHandlingMiddleware(
        ILogger<ErrorHandlingMiddleware> logger
        )
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException e)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(e.Message);
        }
        catch (UnauthorizedException e)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(e.Message);
        }
        catch (ForbidException e)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(e.Message);
        }
        catch (NotFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Resource not found");
        }
        catch (NoResultException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Resource not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}