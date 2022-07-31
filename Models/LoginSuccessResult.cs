using System.Text.Json.Serialization;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Models;

public class LoginSuccessResult
{
    public string AuthToken { get; set; }
    public string Uid { get; set; }
}