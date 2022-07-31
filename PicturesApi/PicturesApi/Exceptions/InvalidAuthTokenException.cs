namespace PicturesAPI.Exceptions;

public class InvalidAuthTokenException : System.Exception
{
    public override string Message { get; } = "Invalid authentication token. Try re-logging";
}