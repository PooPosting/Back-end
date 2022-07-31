using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos.Picture;

public class UpdatePictureNameDto
{
    [Required]
    [MaxLength(40)]
    public string Name { get; set; }
}