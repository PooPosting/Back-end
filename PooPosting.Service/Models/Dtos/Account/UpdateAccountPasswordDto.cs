using System.ComponentModel.DataAnnotations;

namespace PooPosting.Application.Models.Dtos.Account;

public class UpdateAccountPasswordDto
{
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = null!;
    [Required]
    public string ConfirmPassword { get; set; } = null!;
}