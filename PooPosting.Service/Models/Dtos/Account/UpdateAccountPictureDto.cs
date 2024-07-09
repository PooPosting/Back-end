using Microsoft.AspNetCore.Http;

namespace PooPosting.Service.Models.Dtos.Account;

public class UpdateAccountPictureDto
{
    public IFormFile? File { get; set; }
}