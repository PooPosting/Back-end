using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Services.Interfaces;

public interface ILogsService
{
    List<LogDirDto> GetLogsTree();
    string GetLog(string folder, string file);
}