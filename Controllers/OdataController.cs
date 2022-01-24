using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Route("api")]
public class OdataController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IPictureService _pictureService;

    public OdataController(IAccountService accountService, IPictureService pictureService)
    {
        _accountService = accountService;
        _pictureService = pictureService;
    }
    
    [HttpGet]
    [EnableQuery]
    [Route("account/odata")]
    public IActionResult GetAllOdata()
    {
        var result = _accountService.GetAllOdata();
        return Ok(result);
    }
    
    [HttpGet]
    [EnableQuery]
    [Route("picture/odata")]
    public IActionResult GetAllOData()
    {
        var pictures = _pictureService.GetAllOdata();
        return Ok(pictures);
    }
    
}