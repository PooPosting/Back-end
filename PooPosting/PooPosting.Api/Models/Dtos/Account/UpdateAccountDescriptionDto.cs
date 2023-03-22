using System.ComponentModel.DataAnnotations;

namespace PooPosting.Api.Models.Dtos.Account;

public class UpdateAccountDescriptionDto
{
    [MaxLength(500)]
    public string Description { get; set; }
}