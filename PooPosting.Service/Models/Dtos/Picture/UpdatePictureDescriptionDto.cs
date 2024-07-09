using System.ComponentModel.DataAnnotations;

namespace PooPosting.Service.Models.Dtos.Picture;

public class UpdatePictureDescriptionDto
{
    [MaxLength(500)]
    public string Description { get; set; }
}