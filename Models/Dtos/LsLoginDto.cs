using System.IdentityModel.Tokens.Jwt;

namespace PicturesAPI.Models.Dtos;

public class LsLoginDto
{
    public string JwtToken { get; set; }
    public string Guid { get; set; }
}