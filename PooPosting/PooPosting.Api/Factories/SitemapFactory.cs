using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using PooPosting.Api.Entities;
using PooPosting.Api.Factories.Interfaces;
using PooPosting.Api.Models.Configuration;
using PooPosting.Api.Services.Helpers;
using X.Web.Sitemap;

namespace PooPosting.Api.Factories;

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

    public async Task<string> GenerateSitemap()
    {
        var sitemap = new Sitemap();
        foreach (var site in _sitemapSettings.Sites)
        {
            sitemap.Add(CreateUrl(site.Replace("#origin#", _sitemapSettings.Origin)));
        }
        foreach (var picture in await _dbContext.Pictures
                     .OrderByDescending(p => p.PictureAdded)
                     .ToListAsync())
        {
            sitemap.Add(CreateUrl(
                _sitemapSettings.PictureRoute
                    .Replace("#origin#", _sitemapSettings.Origin)
                    .Replace("#pictureId#", IdHasher.EncodePictureId(picture.Id)),
                picture.PictureAdded)
            );
        }
        foreach (var account in await _dbContext.Accounts
                     .OrderByDescending(a => a.AccountCreated)
                     .ToListAsync())
        {
            sitemap.Add(CreateUrl(
                _sitemapSettings.AccountRoute
                    .Replace("#origin#", _sitemapSettings.Origin)
                    .Replace("#accountId#", IdHasher.EncodeAccountId(account.Id)),
                account.AccountCreated)
            );
        }
        return sitemap.ToXml();
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