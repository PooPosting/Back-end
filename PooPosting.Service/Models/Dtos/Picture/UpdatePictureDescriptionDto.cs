using System.ComponentModel.DataAnnotations;

namespace PooPosting.Application.Models.Dtos.Picture;

public class UpdatePictureDescriptionDto
{
    [MaxLength(500)]
    public string Description { get; set; }
}