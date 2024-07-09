namespace PooPosting.Application.Models.Dtos.Account;

public class ForgetTokensDto
{
    public string RefreshToken { get; set; } = null!;
    public string Uid { get; set; } = null!;
}