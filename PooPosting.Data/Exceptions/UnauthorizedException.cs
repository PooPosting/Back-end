namespace PooPosting.Data.Exceptions;

public class UnauthorizedException: Exception
{
    public UnauthorizedException(string? message = "Unauthorized") : base(message)
    {

    }
}