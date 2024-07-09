using PooPosting.Service.Models.Dtos.Account;

namespace PooPosting.Service.Models.Dtos.Comment;

public class CommentDto
{
    public string Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentAdded { get; set; }

    public string PictureId { get; set; }
    public AccountDto Account { get; set; }
}