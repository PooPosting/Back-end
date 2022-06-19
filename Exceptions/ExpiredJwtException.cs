namespace PicturesAPI.Exceptions;

public class ExpiredJwtException : Exception
{
    public ExpiredJwtException(string? message) : base(message)
    {

    }
}