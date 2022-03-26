using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountService
{
    AccountDto GetById(Guid id);
    List<LikeDto> GetAccLikes(Guid id);
    PagedResult<AccountDto> GetAll(AccountQuery query);
    bool Update(PutAccountDto dto);
    bool Delete(Guid id);
    public string GetLikedTags();
}