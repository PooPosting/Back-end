using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos.Account;

public class LsLoginDto
{
    [Required]
    public string JwtToken { get; set; }
    [Required]
    public string Uid { get; set; }
}