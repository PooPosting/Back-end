
namespace PicturesAPI.Models;

public class PictureQuery
{
    public string SearchPhrase { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }

    public string OrderBy { get; set; }

    public string LikedTags { get; set; } = " ";

    public int DaysSincePictureAdded { get; set; } = 30;
}