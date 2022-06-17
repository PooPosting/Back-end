#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ICommentRepo
{
    Task<Comment> GetById(Guid commId);
    Task<Guid> Insert(Comment comment);
    Task Update(Comment comment);
    Task Save();
}