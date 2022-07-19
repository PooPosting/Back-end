using PicturesAPI.Models;
using PicturesAPI.Models.Dtos.Comment;
using PicturesAPI.Models.Queries;

namespace PicturesAPI.Services.Interfaces;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetByPictureId(int picId, Query query);
    Task<CommentDto> Create(int picId, string text);
    Task<CommentDto> Update(int commId, string text);
    Task<bool> Delete(int commId);
}