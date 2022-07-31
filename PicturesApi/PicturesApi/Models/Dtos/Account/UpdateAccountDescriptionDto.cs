using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Models.Dtos.Account;

public class UpdateAccountDescriptionDto
{
    [MaxLength(500)]
    public string Description { get; set; }
}