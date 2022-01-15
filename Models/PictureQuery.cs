namespace PicturesAPI.Models;

public class PictureQuery
{
    public string SearchPhrase { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}