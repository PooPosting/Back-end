using PooPosting.Domain.DbContext.Interfaces;
using PooPosting.Domain.DbContext.Pagination;
using PooPosting.Domain.Enums;

namespace PooPosting.Application.Models.Queries;

public class PictureQueryParams: IPaginationParameters
{
    public string? SearchPhrase { get; set; }
    public OrderBy OrderBy { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}