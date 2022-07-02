using Microsoft.AspNetCore.Mvc;
using PicturesAPI.Factories.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Route("api")]
public class SitemapController : ControllerBase
{
    private readonly ISitemapFactory _sitemapFactory;

    public SitemapController(
        ISitemapFactory sitemapFactory
        )
    {
        _sitemapFactory = sitemapFactory;
    }

    [Route("sitemap.xml")]
    public async Task<IActionResult> Get()
    {
        await _sitemapFactory.GenerateSitemap();
        var xml = System.IO.File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sitemap.xml"));
        return File(xml, "text/xml");
    }
}