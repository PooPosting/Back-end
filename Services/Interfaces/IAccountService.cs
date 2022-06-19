using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountService
{
    AccountDto GetById(int accountId);
    PagedResult<AccountDto> GetAll(AccountQuery accountQuery);
    List<LikeDto> GetAccLikes(int accountId);
    // string GetLikedTags();

    bool Update(PutAccountDto putAccountDto);
    bool Delete(int pictureId);
    bool DeleteAccPics(int accountId);
}