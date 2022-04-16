namespace PicturesAPI.Models.Dtos;

public class AccountDto
{
    public string Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }

    public ICollection<PictureDto> Pictures { get; set; }
    public ICollection<CommentDto> Comments { get; set; }
    
    public int RoleId { get; set; }
    public DateTime AccountCreated { get; set; }
}