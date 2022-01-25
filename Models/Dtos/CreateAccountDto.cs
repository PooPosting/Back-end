using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos;

public class CreateAccountDto
{
    [Required] [MinLength(4)] [MaxLength(25)]
    public string Nickname { get; set; }
        
    [Required] [MinLength(8)] [MaxLength(40)]
    public string Email { get; set; }
        
    [Required] [MinLength(8)]
    public string Password { get; set; }
        
    [Required] [MinLength(8)] 
    public string ConfirmPassword { get; set; }

    public int RoleId { get; set; } = 1;
}