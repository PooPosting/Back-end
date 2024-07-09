using PooPosting.Application.Models.Dtos.Like;
using PooPosting.Domain.Enums;

namespace PooPosting.Application.Models;

public class LikeResult
{
    public LikeState LikeState { get; set; }
    public IEnumerable<LikeDto>? Likes { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
}