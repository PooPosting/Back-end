using PicturesAPI.Models.Dtos.Account;

namespace PicturesAPI.Models.Dtos.Like;

public class LikeDto
{
    public int Id { get; set; }
    public string PictureId { get; set; }
    public bool IsLike { get; set; }
    public AccountPreviewDto AccountPreview { get; set; }
}