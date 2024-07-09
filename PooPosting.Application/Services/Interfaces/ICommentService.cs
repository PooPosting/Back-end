using PooPosting.Application.Models.Dtos.Comment;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Application.Services.Interfaces;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetByPictureId(int picId, PaginationParameters paginationParameters);
    Task<CommentDto> Create(int picId, string text);
    Task<CommentDto> Update(int commId, string text);
    Task<bool> Delete(int commId);
}