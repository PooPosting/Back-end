﻿using PicturesAPI.Exceptions;

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
        catch (ForbidException forbidException)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(forbidException.Message);
        }
        catch (BadRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(badRequestException.Message);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFoundException.Message);
        }
        catch (InvalidAuthTokenException invalidAuthTokenException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(invalidAuthTokenException.Message);
        }
        catch (RestrictedException restrictedException)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(restrictedException.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
        
}