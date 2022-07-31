using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace PicturesAPI.Models.Dtos.Picture;

public class CreatePictureDto
{
    [MaxLength(40)]
    public string Name { get; set; }
    [MaxLength(500)]
    [CanBeNull] public string Description { get; set; }
    [MaxLength(4)]
    [CanBeNull] public List<string> Tags { get; set; }

    [MaxLength(4194304)]
    public IFormFile File { get; set; }
}