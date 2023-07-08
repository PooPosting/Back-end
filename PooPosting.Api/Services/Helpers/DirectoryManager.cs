namespace PooPosting.Api.Services.Helpers;

public static class DirectoryManager
{
    public static void EnsureAllDirectoriesAreCreated()
    {
        var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        if (!Directory.Exists(wwwRootPath)) Directory.CreateDirectory(wwwRootPath);

        var picturesPath = Path.Combine(wwwRootPath, "pictures");
        if (!Directory.Exists(picturesPath)) Directory.CreateDirectory(picturesPath);

        var accountsPath = Path.Combine(wwwRootPath, "accounts");
        if (!Directory.Exists(accountsPath)) Directory.CreateDirectory(accountsPath);

        var accountPfpsPath = Path.Combine(accountsPath, "profile_pictures");
        if (!Directory.Exists(accountPfpsPath)) Directory.CreateDirectory(accountPfpsPath);

        var accountBgpsPath = Path.Combine(accountsPath, "background_pictures");
        if (!Directory.Exists(accountBgpsPath)) Directory.CreateDirectory(accountBgpsPath);
    }
}