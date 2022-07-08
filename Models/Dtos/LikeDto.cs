namespace PicturesAPI.Models.Dtos;

public class LikeDto
{
    public int Id { get; set; }
    public string PictureId { get; set; }
    public bool IsLike { get; set; }
    public AccountPreviewDto AccountPreview { get; set; }
}