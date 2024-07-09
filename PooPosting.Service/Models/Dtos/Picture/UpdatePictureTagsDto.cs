using System.ComponentModel.DataAnnotations;

namespace PooPosting.Service.Models.Dtos.Picture;

public class UpdatePictureTagsDto
{
    [Required]
    [MaxLength(4)]
    public string[] Tags { get; set; }
}