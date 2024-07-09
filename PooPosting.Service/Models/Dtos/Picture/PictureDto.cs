using PooPosting.Service.Models.Dtos.Account;
using PooPosting.Service.Models.Dtos.Comment;

namespace PooPosting.Service.Models.Dtos.Picture;

public class PictureDto
{
    public string Id { get; init; } = null!;
    public IEnumerable<string>? Tags { get; set; }
    public string? Description { get; set; }
    public AccountDto Account { get; set; } = null!;
    public IEnumerable<CommentDto>? Comments { get; set; }
    public string Url { get; set; } = null!;
    public DateTime PictureAdded { get; set; }
    public bool IsLiked { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
}