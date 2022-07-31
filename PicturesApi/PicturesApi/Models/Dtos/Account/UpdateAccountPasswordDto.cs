using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos.Account;

public class UpdateAccountPasswordDto
{
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}