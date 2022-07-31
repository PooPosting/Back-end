using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos.Account;

public class LoginDto
{
    [Required]
    public string Nickname { get; set; }
    [Required]
    public string Password { get; set; }
}