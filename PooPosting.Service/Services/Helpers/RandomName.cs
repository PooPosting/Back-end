namespace PooPosting.Service.Services.Helpers;

public static class RandomName
{
    public static string Generate()
    {
        return Path.GetRandomFileName().Replace('.', '-');
    }
    
    public static string Generate(string extension)
    {
        return $"{Path.GetRandomFileName().Replace('.', '-')}.{extension}";
    }
}