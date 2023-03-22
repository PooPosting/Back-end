using PooPosting.Api.Models.Dtos.Account;

namespace PooPosting.Api.Models.Dtos.Picture;

public class PopularContentDto
{
    public IEnumerable<AccountDto> MostPostedAccounts { get; set; }
    public IEnumerable<AccountDto> MostLikedAccounts { get; set; }

    public IEnumerable<PictureDto> MostLikedPictures { get; set; }
    public IEnumerable<PictureDto> MostVotedPictures { get; set; }
    public IEnumerable<PictureDto> MostCommentedPictures { get; set; }
}