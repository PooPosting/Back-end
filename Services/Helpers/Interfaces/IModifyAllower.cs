using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Helpers.Interfaces;

public interface IModifyAllower
{
    void UpdateItems(IEnumerable<PictureDto> items);
    void UpdateItems(PictureDto item);
    void UpdateItems(IEnumerable<AccountDto> accounts);
    void UpdateItems(AccountDto account);
}