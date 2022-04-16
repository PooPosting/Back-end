namespace PicturesAPI.Models.Dtos;

public class LikeDto
{
    public int Id { get; set; }
    public string AccountNickname { get; set; }
    public string AccountId { get; set; }
    public string PictureId { get; set; }
    public bool IsLike { get; set; }
}