namespace PooPosting.Application.Models.Configuration;

public class HashIdsSettings
{
    public int MinHashLength { get; set; }
    public string HashAlphabet { get; set; }
    public string PictureSalt { get; set; }
    public string AccountSalt { get; set; }
    public string CommentSalt { get; set; }
}