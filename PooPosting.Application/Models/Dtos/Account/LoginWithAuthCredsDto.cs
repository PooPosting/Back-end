using System.ComponentModel.DataAnnotations;

namespace PooPosting.Application.Models.Dtos.Account;

public class LoginWithAuthCredsDto
{
    [Required]
    public string Nickname { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}