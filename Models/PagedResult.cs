namespace PicturesAPI.Models;

public class PagedResult<T>
{
    public PagedResult(List<T> items, int pageSize, int pageNumber, int totalItemsCount)
    {
        Items = items;
        Page = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalItemsCount /(double) pageSize);
    }
    
    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    public int TotalPages { get; set; }

}