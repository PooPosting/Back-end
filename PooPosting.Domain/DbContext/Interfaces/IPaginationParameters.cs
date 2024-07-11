namespace PooPosting.Domain.DbContext.Interfaces;

public interface IPaginationParameters
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}