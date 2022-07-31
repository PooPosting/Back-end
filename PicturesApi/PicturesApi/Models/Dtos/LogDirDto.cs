namespace PicturesAPI.Models.Dtos;

public class LogDirDto
{
    public string Name { get; set; }
    public IEnumerable<string> Files { get; set; }
}