using System.ComponentModel.DataAnnotations;

namespace PooPosting.Service.Models.Dtos.Picture;

public class CreatePictureDto
{
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(4)]
    public string[]? Tags { get; set; }

    [MaxLength(4194304)] public string DataUrl { get; set; } = null!;
}