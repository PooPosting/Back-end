using Microsoft.AspNetCore.Mvc;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Route("api/popular")]
public class PopularController : ControllerBase
{
    private readonly IPopularService _popularService;

    public PopularController(
        IPopularService popularService)
    {
        _popularService = popularService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPopularContent()
    {
        return Ok(await _popularService.Get());
    }
}