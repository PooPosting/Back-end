using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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

    public async Task GenerateSitemap()
    {
        var sitemap = new Sitemap();
        foreach (var site in _sitemapSettings.Sites)
        {
            sitemap.Add(CreateUrl(site.Replace("#origin#", _sitemapSettings.Origin)));
        }
        foreach (var picture in await _dbContext.Pictures.ToArrayAsync())
        {
            sitemap.Add(CreateUrl(
                _sitemapSettings.PictureRoute
                    .Replace("#origin#", _sitemapSettings.Origin)
                    .Replace("#pictureId#", IdHasher.EncodePictureId(picture.Id)),
                picture.PictureAdded)
            );
        }
        foreach (var account in await _dbContext.Accounts.ToArrayAsync())
        {
            sitemap.Add(CreateUrl(
                _sitemapSettings.AccountRoute
                    .Replace("#origin#", _sitemapSettings.Origin)
                    .Replace("#accountId#", IdHasher.EncodeAccountId(account.Id)),
                account.AccountCreated)
            );
        }
        await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sitemap.xml"),
            sitemap.ToXml(), Encoding.UTF8);
    }

    private static Url CreateUrl(string url)
    {
        return new Url
        {
            ChangeFrequency = ChangeFrequency.Never,
            Location = url,
            TimeStamp = DateTime.Now,
            Priority = 0.5
        };
    }

    private static Url CreateUrl(string url, DateTime siteCreated)
    {
        return new Url
        {
            ChangeFrequency = ChangeFrequency.Daily,
            Location = url,
            TimeStamp = siteCreated,
            Priority = 0.5
        };
    }
}