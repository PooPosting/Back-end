using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Models.Dtos.Comment;

namespace PooPosting.Application.Models.Dtos.Picture;

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