namespace PicturesAPI.Models.Dtos;

public class PopularContentDto
{
    public ICollection<AccountDto> MostPostedAccounts { get; set; }
    public ICollection<AccountDto> MostLikedAccounts { get; set; }

    public ICollection<PictureDto> MostLikedPictures { get; set; }
    public ICollection<PictureDto> MostVotedPictures { get; set; }
    public ICollection<PictureDto> MostCommentedPictures { get; set; }
}