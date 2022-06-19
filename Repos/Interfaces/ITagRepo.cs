using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ITagRepo
{
    List<Tag> GetAll();
    List<Tag> GetByPhrase(string phrase);
    int Insert(Tag tag);
    void Delete(Tag tag);
    void Update(Tag tag);
    List<Tag> GetTagsByPictureId(int pictureId);
    List<Tag> GetTagsByAccountId(int accountId);
}
