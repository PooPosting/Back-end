using PicturesAPI.Entities;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;
public class TagRepo : ITagRepo
{
    private readonly PictureDbContext _dbContext;

    public TagRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Tag> GetAll()
    {
        return _dbContext.Tags.ToList();
    }

    public List<Tag> GetByPhrase(string phrase)
    {
        return _dbContext.Tags
            .Where(t => t.Value.Contains(phrase))
            .Take(5)
            .ToList();
    }

    public int Insert(Tag tag)
    {
        _dbContext.Tags.Add(tag);
        return tag.Id;
    }

    public void Delete(Tag tag)
    {
        _dbContext.Tags.Remove(tag);
    }

    public void Update(Tag tag)
    {
        _dbContext.Tags.Update(tag);
    }

    public List<Tag> GetTagsByPictureId(int pictureId)
    {
        return _dbContext.PictureTagJoins
            .Where(p => p.PictureId == pictureId)
            .Select(p => p.Tag)
            .ToList();
    }

    public List<Tag> GetTagsByAccountId(int accountId)
    {
        return _dbContext.AccountLikedTagJoins
            .Where(a => a.AccountId == accountId)
            .Select(t => t.Tag)
            .ToList();
    }

}