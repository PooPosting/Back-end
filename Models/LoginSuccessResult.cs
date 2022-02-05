using System.Text.Json.Serialization;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models;

public class LoginSuccessResult
{
    public AccountDto AccountDto { get; set; }
    public string LikedTags { get; set; }
    public string AuthToken { get; set; }
    
    public ICollection<LikeDto> Likes { get; set; }
}