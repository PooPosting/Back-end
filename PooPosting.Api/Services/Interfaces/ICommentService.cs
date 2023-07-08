using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Queries;

namespace PooPosting.Api.Services.Interfaces;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetByPictureId(int picId, Query query);
    Task<CommentDto> Create(int picId, string text);
    Task<CommentDto> Update(int commId, string text);
    Task<bool> Delete(int commId);
}