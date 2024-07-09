using System.ComponentModel.DataAnnotations;
using PooPosting.Data.DbContext.Entities.Joins;
using PooPosting.Data.DbContext.Interfaces;

namespace PooPosting.Data.DbContext.Entities;

public class Tag: IIdentifiable
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(25)]
    public string Value { get; set; } = null!;
    public ICollection<PictureTag> PictureTags { get; set; } = null!;
    public ICollection<AccountLikedTag> AccountLikedTags { get; set; } = null!;
}