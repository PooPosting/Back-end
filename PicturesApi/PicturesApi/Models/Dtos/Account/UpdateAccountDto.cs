#nullable enable
namespace PicturesAPI.Models.Dtos.Account;

public class UpdateAccountDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Description { get; set; }

    public IFormFile? ProfilePic { get; set; }
    public IFormFile? BackgroundPic { get; set; }
}