using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Models.Dtos.Account;

public class LoginWithAuthCredsDto
{
    [Required]
    public string Nickname { get; set; }
    [Required]
    public string Password { get; set; }
}