using System.Net.Mime;
using System.Text;
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
    [Consumes(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> Get()
    {
        var sitemap = await _sitemapFactory.GenerateSitemap();
        return Ok(Encoding.UTF8.GetBytes(sitemap.ToXml()));
    }
}