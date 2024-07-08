using PooPosting.Api.Models.Queries;
using PooPosting.Domain.Enums;

namespace PooPosting.Application.Models.Queries;

public class PictureSearchQuery: Query
{
    public string? SearchPhrase { get; set; }
    public OrderBy OrderBy { get; set; }
}