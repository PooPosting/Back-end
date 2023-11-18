using System.Text.Json.Serialization;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Models;

public class AuthSuccessResult
{
    public string AuthToken { get; set; }
    public string RefreshToken { get; set; }
    public string Uid { get; set; }
    public int RoleId { get; set; }
}