using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Models;

namespace PooPosting.Application.Services.Interfaces;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetByPictureId(int picId, Query query);
    Task<CommentDto> Create(int picId, string text);
    Task<CommentDto> Update(int commId, string text);
    Task<bool> Delete(int commId);
}