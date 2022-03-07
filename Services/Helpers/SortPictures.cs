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
        var result = 0.0;
        var date = DateTime.Today.AddDays(-1);

        var intersectedTags = picture.Tags
            .Split(' ')
            .Intersect(query.LikedTags.Split(' '));

        if ((DateTime.Now - picture.PictureAdded).TotalMinutes < 30)
        {
            result += (DateTime.Now - date).TotalMinutes * 1;
            result -= (DateTime.Now - picture.PictureAdded).TotalMinutes * 0.7;
        }
        else if (picture.PictureAdded > date)
        {
            result += (DateTime.Now - date).TotalMinutes * 0.6;
            result -= (DateTime.Now - picture.PictureAdded).TotalMinutes * 0.3;
        }
        else
        {
            result += 250;
        }
        
        for (int i = 0; i < picture.Likes.Count(l => l.IsLike); i++)
        {
            result *= 1.075;
        }
        for (int i = 0; i < picture.Likes.Count(l => l.IsLike == false); i++)
        {
            result *= 1.02;
        }
        
        return intersectedTags.Aggregate(result, (current, tag) => current * 1.15);
    }
}