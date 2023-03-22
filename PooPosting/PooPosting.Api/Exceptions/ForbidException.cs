#nullable enable
namespace PooPosting.Api.Exceptions;

public class ForbidException : System.Exception
{
    public ForbidException(string? message) : base(message)
    {
        
    }
}