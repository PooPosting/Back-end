using System.ComponentModel.DataAnnotations;

namespace PooPosting.Application.Models.Dtos.Account;

public class UpdateAccountEmailDto
{
    [Required]
    [MaxLength(40)]
    public string Email { get; set; } = null!;
}