using Microsoft.OData.Edm;
using PicturesAPI.Entities;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Services.Helpers;

public static class SortPictures
{
    public static List<Picture> SortPics(List<Picture> pictures, PictureQuery query)
    {
        var result = pictures
            .OrderByDescending(p => CountPicPoints(p, query))
            .ToList();
        return result;
    }

    private static double CountPicPoints(Picture picture, PictureQuery query)
    {
        var result = 10.0;
        var date = DateTime.Today.AddDays(-1);

        var intersectedTags = picture.Tags
            .Split(' ')
            .Intersect(query.LikedTags.Split(' '));
        
        if (picture.PictureAdded > date)
        {
            result += (((DateTime.Now - date).Days * 1440) + ((DateTime.Now - date).Hours * 60) + (DateTime.Now - date).Minutes) * 0.1;
            result += (((picture.PictureAdded - date).Days * 1440) + ((picture.PictureAdded - date).Hours * 60) + (picture.PictureAdded - date).Minutes) * 0.2;
        }
        else
        {
            result += 750;
        }
        for (int i = 0; i < picture.Likes.Count(l => l.IsLike); i++)
        {
            result *= 1.025;
        }
        for (int i = 0; i < picture.Likes.Count(l => l.IsLike == false); i++)
        {
            result *= 1.005;
        }
        
        return intersectedTags.Aggregate(result, (current, tag) => current * 1.15);
    }
}