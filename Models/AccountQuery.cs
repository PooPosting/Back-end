namespace PicturesAPI.Models;

public class AccountQuery
{
    public string SearchPhrase { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    
}