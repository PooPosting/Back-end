namespace PicturesAPI.Exceptions;

public class RestrictedException: Exception
{
    public RestrictedException(string message) : base(message)
    {
            
    }
}