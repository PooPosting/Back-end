using PooPosting.Data.DbContext.Pagination;
using PooPosting.Data.Enums;

namespace PooPosting.Service.Models.Queries;

public class PictureQueryParams: PaginationParameters
{
    public string? SearchPhrase { get; set; }
    public OrderBy OrderBy { get; set; }
}