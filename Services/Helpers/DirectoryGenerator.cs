namespace PicturesAPI.Services.Helpers;

public static class DirectoryGenerator
{
    public static string GetLogsDirectory()
    {
        var rootDir = Directory.GetCurrentDirectory();
        var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
        if (isDevelopment) rootDir += "/bin/Debug/net6.0/";
        return Path.Combine(rootDir,"logs");
    }
}