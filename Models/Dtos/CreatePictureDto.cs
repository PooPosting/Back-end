#nullable enable

namespace PicturesAPI.Models.Dtos;

public class CreatePictureDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string>? Tags { get; set; }
    public IFormFile? Picture { get; set; }
}