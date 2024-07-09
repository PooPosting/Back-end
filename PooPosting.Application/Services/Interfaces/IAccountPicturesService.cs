using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Application.Services.Interfaces;

public interface IAccountPicturesService
{
    Task<PagedResult<PictureDto>> GetPaged(PaginationParameters paginationParameters, string accountId);
    Task<PagedResult<PictureDto>> GetLikedPaged(PaginationParameters paginationParameters, string accountId);
}