using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Models.Dtos.Comment;

namespace PooPosting.Service.Services.Interfaces;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetByPictureId(int picId, PaginationParameters paginationParameters);
    Task<CommentDto> Create(int picId, string text);
    Task<CommentDto> Update(int commId, string text);
    Task<bool> Delete(int commId);
}