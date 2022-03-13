using PicturesAPI.Entities;

namespace PicturesAPI.Models.Dtos;

public class LikeDto
{
    public int Id { get; set; }
    public string AccountNickname { get; set; }
    public Guid AccountId { get; set; }
    public Guid PictureId { get; set; }
    public bool IsLike { get; set; }
}