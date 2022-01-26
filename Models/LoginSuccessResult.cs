using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models;

public class LoginSuccessResult
{
    public AccountDto AccountDto { get; set; }
    public string AuthToken { get; set; }
}