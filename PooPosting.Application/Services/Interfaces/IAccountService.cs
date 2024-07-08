using PooPosting.Application.Models;
using PooPosting.Application.Models.Dtos.Account;
using PooPosting.Application.Models.Queries;

namespace PooPosting.Application.Services.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetById(int id);
    Task<AccountDto> GetCurrent();
    Task<PagedResult<AccountDto>> GetAll(AccountSearchQuery query);
    Task<AccountDto> UpdateEmail(UpdateAccountEmailDto dto);
    Task<AccountDto> UpdateDescription(UpdateAccountDescriptionDto dto);
    Task<AccountDto> UpdatePassword(UpdateAccountPasswordDto dto);
    Task<AccountDto> UpdateBackgroundPicture(UpdateAccountPictureDto dto);
    Task<AccountDto> UpdateProfilePicture(UpdateAccountPictureDto dto);
    Task<bool> Delete(int id);

}