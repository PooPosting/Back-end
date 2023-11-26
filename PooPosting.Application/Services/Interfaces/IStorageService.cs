namespace PooPosting.Application.Services.Interfaces;

public interface IStorageService
{
    Task UploadFile(string fullPath, byte[] itemBytes);
}