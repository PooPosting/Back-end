using X.Web.Sitemap;

namespace PicturesAPI.Factories.Interfaces;

public interface ISitemapFactory
{
    Task<Sitemap> GenerateSitemap();
}