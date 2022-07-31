namespace PicturesAPI.Entities.Joins;

public class PictureTag
{
    public int Id { get; set; }
    public Picture Picture { get; set; }
    public Tag Tag { get; set; }

    public int PictureId { get; set; }
    public int TagId { get; set; }
}