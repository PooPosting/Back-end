using System.ComponentModel.DataAnnotations;
using PicturesAPI.Entities.Joins;

namespace PicturesAPI.Entities;

public class Tag
{
    public int Id { get; set; }
    [MaxLength(25)]
    public string Value { get; set; }
    public IEnumerable<PictureTag> PictureTags { get; set; }
    public IEnumerable<AccountLikedTag> AccountLikedTags { get; set; }
}