using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PooPosting.Api.Factories.Interfaces;

namespace PooPosting.Api.Controllers;

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
        return Ok(await _sitemapFactory.GenerateSitemap());
    }
}