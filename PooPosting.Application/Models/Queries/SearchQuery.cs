using PooPosting.Api.Models.Queries;
using PooPosting.Domain.Enums;

namespace PooPosting.Application.Models.Queries;

public class SearchQuery: Query
{
    public string? SearchPhrase { get; set; }
    public OrderBy OrderBy { get; set; }
}