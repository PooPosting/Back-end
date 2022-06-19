#nullable enable
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ICommentRepo
{
    Comment GetById(int id);
    int Insert(Comment comment);
    void Update(Comment comment);
    void DeleteById(int guid);
    bool Save();
}