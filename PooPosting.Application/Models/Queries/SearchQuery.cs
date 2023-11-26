using PooPosting.Api.Models.Queries;
using PooPosting.Domain.Enums;

namespace PooPosting.Application.Models.Queries;

public class SearchQuery: Query
{
    public string? SearchPhrase { get; set; }
    public SortBy SearchBy { get; set; }
}