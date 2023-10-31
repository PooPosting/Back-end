using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Dtos.Comment;

namespace PooPosting.Api.Models.Dtos.Picture;

public class PictureDto
{
    public string Id { get; set; }
    public IEnumerable<string> Tags { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public AccountDto Account { get; set; }
    public IEnumerable<CommentDto> Comments { get; set; }

    public string Url { get; set; }
    public DateTime PictureAdded { get; set; }
    public bool IsLiked { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
}