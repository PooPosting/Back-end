using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.ActionFilters;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/admin")]
[ServiceFilter(typeof(IsUserAdminFilter))]
public class AdminController: ControllerBase
{
    private readonly ILogsService _logsService;
    private readonly IRestrictedIpsService _restrictedIpsService;

    public AdminController(
        ILogsService logsService,
        IRestrictedIpsService restrictedIpsService)
    {
        _logsService = logsService;
        _restrictedIpsService = restrictedIpsService;
    }

    [HttpGet]
    [Route("logs")]
    public IActionResult GetLogTree()
    {
        var result = _logsService.GetLogsTree();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("logs/{folder}/{file}")]
    public IActionResult GetLog([FromRoute] string folder, [FromRoute] string file)
    {
        var result = _logsService.GetLog(folder, file);
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("restrictedIps")]
    public IActionResult GetRestrictedIps()
    {
        var result = _restrictedIpsService.GetAll();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("restrictedIp/{ip}")]
    public IActionResult GetRestrictedIp([FromRoute] string ip)
    {
        var result = _restrictedIpsService.GetByIp(ip);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("restrictedIp")]
    public IActionResult AddRestrictedIp([FromBody] RestrictedIp restrictedIp)
    {
        _restrictedIpsService.Add(restrictedIp.IpAddress, restrictedIp.CantGet, restrictedIp.CantPost);
        return NoContent();
    }
    
    [HttpPatch]
    [Route("restrictedIp")]
    public IActionResult UpdateRestrictedIps([FromBody] PatchRestrictedIp restrictedIp)
    {
        _restrictedIpsService.UpdateMany(restrictedIp.Ips, restrictedIp.CantGet, restrictedIp.CantPost);
        return NoContent();
    }

}