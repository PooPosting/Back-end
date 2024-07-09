using PooPosting.Domain.DbContext.Pagination;
using PooPosting.Domain.Enums;

namespace PooPosting.Application.Models.Queries;

public class PictureQueryParams: PaginationParameters
{
    public string? SearchPhrase { get; set; }
    public OrderBy OrderBy { get; set; }
}