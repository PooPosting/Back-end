namespace PicturesAPI.Models.Dtos;

public class AccountDto
{
    public string Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }

    public string ProfilePicUrl { get; set; }
    public string BackgroundPicUrl { get; set; }
    public string AccountDescription { get; set; }

    public bool IsModifiable { get; set; } = false;
    public bool IsAdminModifiable { get; set; } = false;

    public IEnumerable<PicturePreviewDto> PicturePreviews { get; set; }
    public IEnumerable<CommentDto> Comments { get; set; }
    
    public int RoleId { get; set; }
    public DateTime AccountCreated { get; set; }
}