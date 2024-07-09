namespace PooPosting.Service.Models.Dtos.Account;

public class LoginWithRefreshTokenDto
{
    public string RefreshToken { get; set; } = null!;
    public string Uid { get; set; } = null!;
}