#nullable enable
namespace PooPosting.Api.Exceptions;

public class UnauthorizedException: Exception
{
    public UnauthorizedException(string? message = "Unauthorized") : base(message)
    {

    }
}