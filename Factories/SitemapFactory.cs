using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Factories.Interfaces;
using PicturesAPI.Models.Configuration;
using PicturesAPI.Services.Helpers;
using X.Web.Sitemap;

namespace PicturesAPI.Factories;

public class SitemapFactory : ISitemapFactory
{
    private readonly PictureDbContext _dbContext;
    private readonly SitemapSettings _sitemapSettings;

    public SitemapFactory(
        PictureDbContext dbContext,
        SitemapSettings sitemapSettings
        )
    {
        _dbContext = dbContext;
        _sitemapSettings = sitemapSettings;
    }

    public async Task<Sitemap> GenerateSitemap()
    {
        var sitemap = new Sitemap();

        foreach (var site in _sitemapSettings.Sites)
        {
            sitemap.Add(CreateUrl(site.Replace("#origin#", _sitemapSettings.Origin), 1)
            );
        }

        foreach (var picture in await _dbContext.Pictures.ToArrayAsync())
        {
            sitemap.Add(CreateUrl(
                _sitemapSettings.PictureRoute
                    .Replace("#origin#", _sitemapSettings.Origin)
                    .Replace("#pictureId#", IdHasher.EncodePictureId(picture.Id)),
                0.5,
                picture.PictureAdded)
            );
        }

        foreach (var account in await _dbContext.Accounts.ToArrayAsync())
        {
            sitemap.Add(CreateUrl(
                _sitemapSettings.AccountRoute
                    .Replace("#origin#", _sitemapSettings.Origin)
                    .Replace("#accountId#", IdHasher.EncodeAccountId(account.Id)),
                0.5,
                account.AccountCreated)
            );
        }

        return sitemap;
    }

    private static Url CreateUrl(string url, double priority)
    {
        return new Url
        {
            ChangeFrequency = ChangeFrequency.Weekly,
            Location = url,
            Priority = priority,
            TimeStamp = DateTime.Now
        };
    }

    private static Url CreateUrl(string url, double priority, DateTime siteCreated)
    {
        return new Url
        {
            ChangeFrequency = ChangeFrequency.Daily,
            Location = url,
            Priority = priority,
            TimeStamp = siteCreated
        };
    }
}