#nullable enable
using PooPosting.Api.Models.Dtos.Picture;

namespace PooPosting.Api.Models.Dtos.Account;

public class AccountDto
{
    public string Id { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? ProfilePicUrl { get; set; }

    public IEnumerable<PictureDto> Pictures { get; set; } = new List<PictureDto>();
    public int PictureCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int CommentCount { get; set; } = 0;
    
    public int RoleId { get; set; }
    public DateTime AccountCreated { get; set; }
}