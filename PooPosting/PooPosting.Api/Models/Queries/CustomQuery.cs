using PooPosting.Api.Enums;

namespace PooPosting.Api.Models.Queries;

public class CustomQuery
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string SearchPhrase { get; set; }
    public SortBy SearchBy { get; set; }
}