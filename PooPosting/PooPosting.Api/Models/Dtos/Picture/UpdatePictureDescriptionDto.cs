using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Models.Dtos.Picture;

public class UpdatePictureDescriptionDto
{
    [MaxLength(500)]
    public string Description { get; set; }
}