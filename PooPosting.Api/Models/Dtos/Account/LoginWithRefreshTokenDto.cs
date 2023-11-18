namespace PooPosting.Api.Models.Dtos.Account;

public class LoginWithRefreshTokenDto
{
    public string RefreshToken { get; set; }
    public string Uid { get; set; }
}