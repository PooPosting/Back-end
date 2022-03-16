using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureCommentService
{
    CommentDto CreateComment(Guid picId, string text);
    CommentDto ModifyComment(Guid commId, string text);
    bool DeleteComment(Guid commId);
}