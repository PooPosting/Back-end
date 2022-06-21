namespace PicturesAPI.Entities.Joins;

public class AccountLikedTagsJoin
{
    public int Id { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }
}