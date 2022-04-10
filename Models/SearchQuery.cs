using PicturesAPI.Enums;

namespace PicturesAPI.Models;

public class SearchQuery : PictureQuery
{
    public string SearchPhrase { get; set; }
    public SortSearchBy SearchBy { get; set; }
}