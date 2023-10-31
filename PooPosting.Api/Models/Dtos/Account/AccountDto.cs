using PooPosting.Api.Models.Dtos.Picture;

namespace PooPosting.Api.Models.Dtos.Account;

public class AccountDto
{
    public string Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }

    public string? ProfilePicUrl { get; set; }
    public string BackgroundPicUrl { get; set; }
    public string AccountDescription { get; set; }

    public IEnumerable<PictureDto> Pictures { get; set; } = new List<PictureDto>();
    public int PictureCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int CommentCount { get; set; } = 0;
    
    public int RoleId { get; set; }
    public DateTime AccountCreated { get; set; }
}