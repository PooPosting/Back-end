namespace PooPosting.Application.Models;

public class AuthSuccessResult
{
    public string AuthToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string Uid { get; set; } = null!;
    public int RoleId { get; set; }
}