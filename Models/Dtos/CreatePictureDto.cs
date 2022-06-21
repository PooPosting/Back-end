using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos;

public class CreatePictureDto
{

    [Required] [MinLength(4)] [MaxLength(25)]
    public string Name { get; set; }
        
    [MaxLength(400)]
    public string? Description { get; set; }
        
    [MaxLength(4)]
    public List<string>? Tags { get; set; }
    
}