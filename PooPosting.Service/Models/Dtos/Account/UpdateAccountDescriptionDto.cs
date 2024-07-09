using System.ComponentModel.DataAnnotations;

namespace PooPosting.Service.Models.Dtos.Account;

public class UpdateAccountDescriptionDto
{
    [MaxLength(500)]
    public string Description { get; set; } = null!;
}