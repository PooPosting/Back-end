using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Models.Dtos.Account;
using PooPosting.Service.Models.Queries;

namespace PooPosting.Service.Services.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetById(int id);
    Task<AccountDto> GetCurrent();
    Task<PagedResult<AccountDto>> GetAll(AccountQueryParams paginationParameters);
    Task<AccountDto> UpdateEmail(UpdateAccountEmailDto dto);
    Task<AccountDto> UpdatePassword(UpdateAccountPasswordDto dto);
    Task<AccountDto> UpdateBackgroundPicture(UpdateAccountPictureDto dto);
    Task<AccountDto> UpdateProfilePicture(UpdateAccountPictureDto dto);
    Task<bool> Delete(int id);

}