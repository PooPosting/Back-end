using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Interfaces;

public interface IPictureCommentService
{
    CommentDto Create(int picId, string text);
    CommentDto Update(int commId, string text);
    void Delete(int commId);
}