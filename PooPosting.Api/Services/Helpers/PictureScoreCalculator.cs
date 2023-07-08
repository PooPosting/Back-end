using PooPosting.Api.Entities;

namespace PooPosting.Api.Services.Helpers;
public static class PictureScoreCalculator
{
    public static long CalcPoints(Picture picture)
    {
        double result = 0;
        var date = DateTime.Today.AddDays(-7);
        var time = (date - DateTime.Now).TotalMinutes;
        var likePoints = 1.0;

        picture.Likes.ToList().ForEach(l =>
        {
            if (l.IsLike) likePoints += 1;
            else likePoints += 0.5;
        });

        if ((DateTime.Now - picture.PictureAdded).TotalMinutes < 180)
        {
            result += CalcPicPoints(likePoints, time) * 1.5;
        }
        else if (picture.PictureAdded > date)
        {
            result += CalcPicPoints(likePoints, time);
        }
        else
        {
            result += CalcPicPoints(likePoints, time)*0.35;
        }
        return (long)result;
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