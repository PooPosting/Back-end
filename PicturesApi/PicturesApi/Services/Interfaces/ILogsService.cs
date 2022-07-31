using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface ILogsService
{
    List<LogDirDto> GetLogsTree();
    string GetLog(string folder, string file);
}