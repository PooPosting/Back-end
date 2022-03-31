using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models;

public class LsLoginSuccessResult
{
    public AccountDto AccountDto { get; set; }
    public string LikedTags { get; set; }
    public ICollection<LikeDto> Likes { get; set; }
}