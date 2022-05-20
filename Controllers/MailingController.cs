using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.ActionFilters;
using PicturesAPI.Models;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/mail")]
[ServiceFilter(typeof(IsIpRestrictedFilter))]
[ServiceFilter(typeof(IsIpBannedFilter))]
public class MailingController : ControllerBase
{
    private readonly IEmailService _emailService;
    public MailingController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    [HttpPost]
    public IActionResult SendLogs([FromBody] EmailData emailData)
    {
        var res = _emailService.SendLogsAsEmail(emailData);
        return res ?  Ok(true) : BadRequest(false);
    }

    // [HttpPost]
    // public IActionResult SendVerificationEmail()
    // {
    //     return Ok();
    // }
}