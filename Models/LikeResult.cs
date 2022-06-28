using PicturesAPI.Enums;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models;

public class LikeResult
{
    public LikeState LikeState { get; set; }
    public IEnumerable<LikeDto> Likes { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
}