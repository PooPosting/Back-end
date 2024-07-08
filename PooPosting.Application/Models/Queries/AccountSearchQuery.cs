using PooPosting.Api.Models.Queries;

namespace PooPosting.Application.Models.Queries;

public class AccountSearchQuery: Query
{
    public string? SearchPhrase { get; set; }
}