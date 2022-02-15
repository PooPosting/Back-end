using PicturesAPI.Enums;

namespace PicturesAPI.Models;

public class SearchQuery
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string SearchPhrase { get; set; }
    public SortSearchBy SearchBy { get; set; }
}