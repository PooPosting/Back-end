using System.ComponentModel.DataAnnotations;
using PicturesAPI.Entities.Joins;

namespace PicturesAPI.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Value { get; set; }
    public ICollection<PictureTag> PictureTags { get; set; }
    public ICollection<AccountLikedTag> AccountLikedTags { get; set; }
}