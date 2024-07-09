using PooPosting.Application.Models.Dtos.Like;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Application.Services.Interfaces;

public interface ILikeService
{
    Task<PagedResult<LikeDto>> GetLikesByPictureId(PaginationParameters paginationParameters, int picId);
}