using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetById(int id);
    Task<PagedResult<AccountDto>> GetAll(AccountQuery query);
    Task<List<LikeDto>> GetAccLikes(int id);
    Task<AccountDto> Update(UpdateAccountDto dto);
    Task<bool> Delete(int id);
    Task<IEnumerable<PictureDto>> DeleteAccPics(int id);
}