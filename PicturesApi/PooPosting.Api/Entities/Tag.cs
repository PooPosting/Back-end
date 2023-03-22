using System.ComponentModel.DataAnnotations;
using PooPosting.Api.Entities.Joins;

namespace PooPosting.Api.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Value { get; set; }
    public ICollection<PictureTag> PictureTags { get; set; }
    public ICollection<AccountLikedTag> AccountLikedTags { get; set; }
}