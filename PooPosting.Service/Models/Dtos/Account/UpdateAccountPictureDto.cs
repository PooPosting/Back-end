using Microsoft.AspNetCore.Http;

namespace PooPosting.Application.Models.Dtos.Account;

public class UpdateAccountPictureDto
{
    public IFormFile? File { get; set; }
}