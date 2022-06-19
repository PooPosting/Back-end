using System.ComponentModel.DataAnnotations;
using PicturesAPI.Entities.Joins;

namespace PicturesAPI.Entities;

public class Tag
{
    public int Id { get; set; }
    [MaxLength(25)]
    public string Value { get; set; }
    public ICollection<PictureTagJoin> PictureTagJoins { get; set; }
    public ICollection<AccountLikedTagJoin> AccountLikedTagJoins { get; set; }
}