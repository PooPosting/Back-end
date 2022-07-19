namespace PicturesAPI.Models;

public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int pageNumber, int totalItems)
    {
        Items = items;
        Page = pageNumber;
        TotalItems = totalItems;
    }
    
    public IEnumerable<T> Items { get; set; }
    public int Page { get; set; }
    public int TotalItems { get; set; }

}