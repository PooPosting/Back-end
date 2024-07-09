using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Application.Models.Queries;

public class AccountQueryParams: PaginationParameters
{
    public string? SearchPhrase { get; set; }
}