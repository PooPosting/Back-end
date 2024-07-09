namespace PooPosting.Service.Services.Interfaces;

public interface IStorageService
{
    Task<string> UploadFile(string dataUrl, string filePath);
}