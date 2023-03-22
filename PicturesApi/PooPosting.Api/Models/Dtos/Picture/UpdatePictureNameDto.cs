using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Models.Dtos.Picture;

public class UpdatePictureNameDto
{
    [Required]
    [MaxLength(40)]
    public string Name { get; set; }
}