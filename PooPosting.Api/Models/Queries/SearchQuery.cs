using PooPosting.Api.Enums;

namespace PooPosting.Api.Models.Queries;

public class SearchQuery: Query
{
    public string SearchPhrase { get; set; }
    public SortBy SearchBy { get; set; }
}