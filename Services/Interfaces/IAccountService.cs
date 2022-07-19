using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Account;
using PicturesAPI.Models.Dtos.Like;
using PicturesAPI.Models.Queries;

namespace PicturesAPI.Services.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetById(int id);
    Task<PagedResult<AccountDto>> GetAll(CustomQuery query);
    Task<AccountDto> UpdateEmail(UpdateAccountEmailDto dto);
    Task<AccountDto> UpdateDescription(UpdateAccountDescriptionDto dto);
    Task<AccountDto> UpdatePassword(UpdateAccountPasswordDto dto);
    Task<AccountDto> UpdateBackgroundPicture(UpdateAccountPictureDto dto);
    Task<AccountDto> UpdateProfilePicture(UpdateAccountPictureDto dto);
    Task<bool> Delete(int id);

}