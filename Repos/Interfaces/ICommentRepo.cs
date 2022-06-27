#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ICommentRepo
{
    Task<Comment?> GetByIdAsync(int commId);
    Task<Comment> InsertAsync(Comment comment);
    Task<Comment> UpdateAsync(Comment comment);
    Task<bool> TryDeleteByIdAsync(int id);
}