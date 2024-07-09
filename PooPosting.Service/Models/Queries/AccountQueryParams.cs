using PooPosting.Data.DbContext.Pagination;

namespace PooPosting.Service.Models.Queries;

public class AccountQueryParams: PaginationParameters
{
    public string? SearchPhrase { get; set; }
}