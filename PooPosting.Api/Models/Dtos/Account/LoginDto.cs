using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Models.Dtos.Account;

public class LoginDto
{
    [Required]
    public string Nickname { get; set; }
    [Required]
    public string Password { get; set; }
}