using System.ComponentModel.DataAnnotations;

namespace PooPosting.Application.Models.Dtos.Account;

public class CreateAccountDto
{
    [Required]
    [MinLength(4)]
    [MaxLength(25)]
    public string Nickname { get; set; } = null!;
        
    [Required] [MinLength(8)] [MaxLength(40)]
    public string Email { get; set; } = null!;
        
    [Required] [MinLength(8)]
    public string Password { get; set; } = null!;
        
    [Required] [MinLength(8)] 
    public string ConfirmPassword { get; set; } = null!;

    public int RoleId { get; set; } = 1;
}