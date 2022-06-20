using Microsoft.OData.Edm;
using PicturesAPI.Entities;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services.Helpers;

public class PictureSortingService
{
    private readonly PictureDbContext _dbContext;
    private readonly IAccountContextService _accountContextService;

    public PictureSortingService(
        PictureDbContext dbContext,
        IAccountContextService accountContextService)
    {
        _dbContext = dbContext;
        _accountContextService = accountContextService;
    }
    public List<Picture> SortPics(IEnumerable<Picture> pictures, PictureQuery query)
    {
        var result = pictures
            .OrderByDescending(p => CountPicPoints(p, query))
            .ToList();
        return result;
    }

    private double CountPicPoints(Picture picture, Account user)
    {
        var result = 0.0;
        var date = DateTime.Today.AddDays(-7);
        var time = (date - DateTime.Now).TotalMinutes;
        var likePoints = 0.0;

        picture.Likes.ToList().ForEach(l =>
        {
            if (l.IsLike) likePoints += 1;
            else likePoints += 0.5;
        });


        var accountLikedTags = picture.PictureTagJoins.Select(j => j.Tag);

        if ((DateTime.Now - picture.PictureAdded).TotalMinutes < 180)
        {
            result += CalcPicPoints(likePoints, time) * 1.75;
        }
        else if (picture.PictureAdded > date)
        {
            result += CalcPicPoints(likePoints, time);
        }
        else
        {
            result += Math.Log(likePoints + 10) * 10;
        }

        return result;
        // return intersectedTags.Aggregate(result, (current, tag) => current * 2.5);
    }
    
    private double CalcPicPointModifier(double x)
    {
        var fx = Math.Log(0.1, 30) * (x + 1);
        return fx;
    }
    private double CalcPicPoints(double likes, double time)
    {
        var gx = CalcPicPointModifier(likes + 10) * (time / 4);
        return gx;
    }
    
}