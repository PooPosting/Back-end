using PicturesAPI.Models.Dtos;
using PicturesAPI.Models.Dtos.Comment;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureCommentService
{
    Task<CommentDto> Create(int picId, string text);
    Task<CommentDto> Update(int commId, string text);
    Task<bool> Delete(int commId);
}