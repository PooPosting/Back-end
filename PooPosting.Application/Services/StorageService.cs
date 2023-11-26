using PooPosting.Application.Services.Interfaces;

namespace PooPosting.Application.Services;

public class StorageService : IStorageService
{
    public async Task UploadFile(string fullPath, byte[] itemBytes)
    {
        try
        {
            await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Write);
            await stream.WriteAsync(itemBytes);
        }
        catch (Exception)
        {
            throw new Exception("Could not upload data");
        }
    }
}