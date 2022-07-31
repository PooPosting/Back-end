using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Authorization;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class LogsService: ILogsService
{
    private readonly IAccountContextService _accountContextService;

    public LogsService(
        IAccountContextService accountContextService)
    {
        _accountContextService = accountContextService;
    }

    public List<LogDirDto> GetLogsTree()
    {
        var rootDir = DirectoryGenerator.GetLogsDirectory();
        var dirs = Directory.GetDirectories(rootDir);
        var logDirs = new List<LogDirDto>();
        
        foreach (var dir in dirs)
        {
            var filesBuffer = Directory.GetFiles(dir).ToList();
            var files = new string[filesBuffer.Count];
            for (var i = 0; i < files.Length; i++)
            {
                files[i] = filesBuffer[i].Split('/', '\\').Last();
            }
            
            logDirs.Add(new LogDirDto()
            {
                Name = dir.Split('/', '\\').Last(),
                Files = files
            });
        }

        return logDirs;
    }
    public string GetLog(string folder, string file)
    {
        var logsDir = DirectoryGenerator.GetLogsDirectory();
        string log;
        try
        {
            using (var sr = new StreamReader($"{logsDir}/{folder}/{file}"))
            {
                log = sr.ReadToEnd();
            }
        }
        catch (IOException)
        {
            throw new NotFoundException();
        }

        return log;
    }

}