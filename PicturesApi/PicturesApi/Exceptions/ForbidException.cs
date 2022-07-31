#nullable enable
namespace PicturesAPI.Exceptions;

public class ForbidException : System.Exception
{
    public ForbidException(string? message) : base(message)
    {
        
    }
}