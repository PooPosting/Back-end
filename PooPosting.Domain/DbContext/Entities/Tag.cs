using System.ComponentModel.DataAnnotations;
using PooPosting.Domain.DbContext.Entities.Joins;

namespace PooPosting.Domain.DbContext.Entities;

public class Tag
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(25)]
    public string Value { get; set; } = null!;
    public ICollection<PictureTag> PictureTags { get; set; } = null!;
    public ICollection<AccountLikedTag> AccountLikedTags { get; set; } = null!;
}