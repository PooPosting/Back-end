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
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sitemap = await _sitemapFactory.GenerateSitemap();
        return File(sitemap.ToXml(), "application/xml");
    }
}