using PicturesAPI.Entities;

namespace PicturesAPI.Services.Helpers;

// this whole class is really processor-heavy due to its methods which calculate score values for every picture in the database
// redesign this or your server will die
public static class PictureSorter
{
    public static List<Picture> SortPics(IEnumerable<Picture> pictures, IEnumerable<Tag> tags)
    {
        var result = pictures
            .OrderByDescending(p => MultiplyPicPoints(p, CountPicPoints(p), tags))
            .ToList();
        return result;
    }

    public static List<Picture> SortPics(IEnumerable<Picture> pictures)
    {
        var result = pictures
            .OrderByDescending(CountPicPoints)
            .ToList();
        return result;
    }

    private static double CountPicPoints(Picture picture)
    {
        double result = 0;
        var date = DateTime.Today.AddDays(-7);
        var time = (date - DateTime.Now).TotalMinutes;
        var likePoints = 0.0;

        picture.Likes.ToList().ForEach(l =>
        {
            if (l.IsLike) likePoints += 1;
            else likePoints += 0.5;
        });

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
    }

    private static double MultiplyPicPoints(Picture picture, double pointCount, IEnumerable<Tag> tags)
    {
        var accountLikedTags = picture.PictureTagJoins.Select(j => j.Tag);

        var picTags = tags as Tag[] ?? tags.ToArray();
        pointCount = accountLikedTags
            .Aggregate(pointCount, (current1, accTag) => picTags
                .Where(picTag => picTag == accTag)
            .Aggregate(current1, (current, picTag) => current * 2.5));
        return pointCount;
    }

    private static double CalcPicPointModifier(double x)
    {
        var fx = Math.Log(0.1, 30) * (x + 1);
        return fx;
    }
    private static double CalcPicPoints(double likes, double time)
    {
        var gx = CalcPicPointModifier(likes + 10) * (time / 4);
        return gx;
    }
    
}