using PicturesAPI.Entities;

namespace PicturesAPI.Repos;

public class TagRepo
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

}