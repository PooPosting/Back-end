
namespace PicturesAPI.Models;

public class PictureQuery
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string LikedTags { get; set; } = " ";

    
    // change this in future
    public int DaysSincePictureAdded { get; set; } = 30;
}