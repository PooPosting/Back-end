using PooPosting.Domain.DbContext.Interfaces;

namespace PooPosting.Application.Models.Queries;

public class AccountQueryParams: IPaginationParameters
{
    public string? SearchPhrase { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}