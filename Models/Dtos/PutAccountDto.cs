using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos;

public class PutAccountDto
{
    public string Email { get; set; }
        
    public string Password { get; set; }
        
    public string ConfirmPassword { get; set; }
}