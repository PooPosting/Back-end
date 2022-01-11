using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models;

public class PutAccountDto
{
    [MinLength(8)] [MaxLength(40)]
    public string Email { get; set; }
        
    [MinLength(8)]
    public string Password { get; set; }
        
    public string ConfirmPassword { get; set; }
}