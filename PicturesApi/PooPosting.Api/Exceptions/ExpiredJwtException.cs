#nullable enable
namespace PooPosting.Api.Exceptions;

public class ExpiredJwtException : Exception
{
    public ExpiredJwtException(string? message) : base(message)
    {

    }
}