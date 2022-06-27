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

    [HttpGet]
    [Route("sitemap.xml")]
    public async Task<IActionResult> Get()
    {
        Response.ContentType = "application/xml";
        return Content((await _sitemapFactory.GenerateSitemap()).ToXml(), "application/xml");
    }
}