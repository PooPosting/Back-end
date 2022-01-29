namespace PicturesAPI.Models;

public class PagedResult<T>
{
    public PagedResult(List<T> items, int totalItemsCount, int pageSize, int pageNumber)
    {
        Items = items;
        Page = pageNumber;
        TotalPages = (int)Math.Ceiling(totalItemsCount /(double) pageSize);
    }
    
    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }

}