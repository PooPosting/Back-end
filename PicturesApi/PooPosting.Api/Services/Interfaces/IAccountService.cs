using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Models.Dtos;
using PooPosting.Api.Models.Dtos.Like;

namespace PooPosting.Api.Services.Interfaces;

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