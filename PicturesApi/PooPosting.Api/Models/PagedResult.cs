namespace PooPosting.Api.Models;

public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
    {
        Items = items;
        Page = pageNumber;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems/(double)pageSize);
    }
    
    public IEnumerable<T> Items { get; set; }
    public int Page { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

}