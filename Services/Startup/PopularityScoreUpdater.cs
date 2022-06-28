using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Services.Startup;

public class PopularityScoreUpdater
{
    private readonly PictureDbContext _dbContext;

    public PopularityScoreUpdater(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update()
    {
        foreach (var picture in _dbContext.Pictures
                     .Include(p => p.Likes)
                     .ToArray())
        {
            picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);
        }
        _dbContext.SaveChanges();
        Console.WriteLine("Pictures updated");
    }
}