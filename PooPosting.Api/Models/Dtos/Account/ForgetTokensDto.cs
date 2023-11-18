namespace PooPosting.Api.Models.Dtos.Account;

public class ForgetTokensDto
{
    public string RefreshToken { get; set; }
    public string Uid { get; set; }
}