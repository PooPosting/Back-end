using PicturesAPI.Enums;

namespace PicturesAPI.Models.Queries;

public class CustomQuery
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string SearchPhrase { get; set; }
    public SortBy SearchBy { get; set; }
}