using PooPosting.Service.Models.Dtos.Account;

namespace PooPosting.Service.Models.Dtos.Like;

public class LikeDto
{
    public int Id { get; set; }
    public string PictureId { get; set; }
    public AccountDto Account { get; set; }
}