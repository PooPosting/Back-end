using PicturesAPI.Entities.Joins;

namespace PicturesAPI.Entities;

public class Picture
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public DateTime PictureAdded { get; set; }
    public bool IsDeleted { get; set; }
    public long PopularityScore { get; set; } = 36500;

    // navigation props
    public int AccountId { get; set; }
    public virtual Account Account { get; set; }
    public virtual ICollection<Like> Likes { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<PictureTag> PictureTags { get; set; }
    public virtual ICollection<PictureSeenByAccount> SeenByAccount { get; set; }
}