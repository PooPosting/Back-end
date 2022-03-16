#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ICommentRepo
{
    Comment? GetComment(Guid commId);
    Guid CreateComment(Guid picId, Guid authorId, string text);
    bool ModifyComment(Guid commId, string text);
    bool DeleteComment(Guid commId);
    bool Exists(Guid id);
}