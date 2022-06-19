namespace PicturesAPI.Entities;

public class PictureTagJoin
{
    public int Id { get; set; }

    public int PictureId { get; set; }
    public Picture Picture { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }
}