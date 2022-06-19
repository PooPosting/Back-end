using Microsoft.EntityFrameworkCore;

namespace PicturesAPI.Entities;

public class PictureSeenByAccountJoin
{
    public int Id { get; set; }

    [Comment("The account user has seen this picture")]
    public Picture Picture { get; set; }
    public int PictureId { get; set; }

    [Comment("The picture was seen by this account user")]
    public Account Account { get; set; }
    public int AccountId { get; set; }
}