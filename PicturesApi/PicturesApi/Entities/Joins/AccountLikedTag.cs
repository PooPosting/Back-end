namespace PicturesAPI.Entities.Joins;

public class AccountLikedTag
{
    public int Id { get; set; }
    public Account Account { get; set; }
    public Tag Tag { get; set; }

    public int AccountId { get; set; }
    public int TagId { get; set; }
}