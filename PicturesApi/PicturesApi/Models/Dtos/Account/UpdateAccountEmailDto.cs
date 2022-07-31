using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos.Account;

public class UpdateAccountEmailDto
{
    [Required]
    [MaxLength(40)]
    public string Email { get; set; }
}