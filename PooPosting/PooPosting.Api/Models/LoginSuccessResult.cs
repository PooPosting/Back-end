using System.Text.Json.Serialization;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Models;

public class LoginSuccessResult
{
    public string AuthToken { get; set; }
    public string Uid { get; set; }
}