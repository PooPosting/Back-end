using PooPosting.Data.Enums;
using PooPosting.Service.Models.Dtos.Like;

namespace PooPosting.Service.Models;

public class LikeResult
{
    public LikeState LikeState { get; set; }
    public IEnumerable<LikeDto>? Likes { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
}