#nullable enable
using System.Text;
using PicturesAPI.Entities;
using PicturesAPI.Models.Configuration;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Helpers;

namespace PicturesAPI.Repos;

public class SitemapRepo : ISitemapRepo
{
    private readonly PictureDbContext _dbContext;
    private readonly SitemapSettings _settings;

    private const string MapTemplate = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" +
                                       "\n\t#content#" +
                                       "\n</urlset>";

    private const string SiteTemplate = "\n<url>" +
                                        "\n\t<loc>#url#</loc>" +
                                        "\n\t<lastmod>#timestamp#</lastmod>" +
                                        "\n</url>";

    public SitemapRepo(
        PictureDbContext dbContext,
        SitemapSettings settings)
    {
        _dbContext = dbContext;
        _settings = settings;
    }

    public async Task UpdateAsync()
    {
        var sites = "";
        foreach (var site in _settings.Sites)
        {
            sites += SiteTemplate
                .Replace("#url#", site)
                .Replace("#origin#", _settings.Origin)
                .Replace("#timestamp#", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK"));
        }
        var accounts = _dbContext.Accounts.ToList();
        foreach (var account in accounts)
        {
            sites += SiteTemplate
                .Replace("#url#", _settings.AccountRoute)
                .Replace("#origin#", _settings.Origin)
                .Replace("#accountId#", IdHasher.EncodeAccountId(account.Id))
                .Replace("#timestamp#", account.AccountCreated.ToString("yyyy-MM-ddTHH:mm:ssK"));
        }
        var pictures = _dbContext.Pictures.ToList();
        foreach (var picture in pictures)
        {
            sites += SiteTemplate
                .Replace("#url#", _settings.PictureRoute)
                .Replace("#origin#", _settings.Origin)
                .Replace("#pictureId#", IdHasher.EncodePictureId(picture.Id))
                .Replace("#timestamp#", picture.PictureAdded.ToString("yyyy-MM-ddTHH:mm:ssK"));
        }

        var result = MapTemplate.Replace("#content#", sites);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "front-assets");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        await using var fs = File.Create(Path.Combine(path, "sitemap.xml"));
        var title = new UTF8Encoding(true).GetBytes(result);
        fs.Write(title, 0, title.Length);
    }

}