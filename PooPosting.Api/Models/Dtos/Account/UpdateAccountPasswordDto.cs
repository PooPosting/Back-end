using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Models.Dtos.Account;

public class UpdateAccountPasswordDto
{
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}