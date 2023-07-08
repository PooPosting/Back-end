using PooPosting.Api.Enums;
using PooPosting.Api.Models.Dtos.Like;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Models;

public class LikeResult
{
    public LikeState LikeState { get; set; }
    public IEnumerable<LikeDto> Likes { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
}