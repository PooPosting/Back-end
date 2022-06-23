using PicturesAPI.Enums;
using PicturesAPI.Models.Interfaces;

namespace PicturesAPI.Models.Dtos;

public class PictureDto
{
    public string Id { get; set; }
    public string AccountId { get; set; }
    public string AccountNickname { get; set; }
    public IEnumerable<string> Tags { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public string Url { get; set; }
    public DateTime PictureAdded { get; set; }

    public IEnumerable<LikeDto> Likes { get; set; }
    public IEnumerable<CommentDto> Comments { get; set; }

    public LikeState LikeState { get; set; } = LikeState.Deleted;
    public bool IsModifiable { get; set; } = false;
    public bool IsAdminModifiable { get; set; } = false;
}