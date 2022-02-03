namespace PicturesAPI.Models.Dtos;

public class LikeDto
{
    public int Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid PictureId { get; set; }
    public bool IsLike { get; set; }
}