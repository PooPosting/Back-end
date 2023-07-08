namespace PooPosting.Api.Entities.Joins;

public class PictureSeenByAccount
{
    public int Id { get; set; }
    public int PictureId { get; set; }
    public Picture Picture { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
}