using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PooPosting.Service.Models.Dtos.Account;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Api.Controllers.Account;

[ApiController]
[Authorize]
[Route("api/account/update")]
public class AccountUpdateController(
    IAccountService accountService
    ) : ControllerBase
{
    [HttpPost]
    [Route("email")]
    public async Task<IActionResult> UpdateAccountEmail(
        [FromBody] UpdateAccountEmailDto dto
    )
    {
        return Ok(await accountService.UpdateEmail(dto));
    }

    [HttpPost]
    [Route("password")]
    public async Task<IActionResult> UpdateAccountPassword(
        [FromBody] UpdateAccountPasswordDto dto
    )
    {
        return Ok(await accountService.UpdatePassword(dto));
    }

    [HttpPatch]
    [Route("profile-picture")]
    public async Task<IActionResult> UpdateAccountProfilePic(
        [FromForm] UpdateAccountPictureDto dto
    )
    {
        return Ok(await accountService.UpdateProfilePicture(dto));

    }

    [HttpPatch]
    [Route("background-picture")]
    public async Task<IActionResult> UpdateAccountBackgroundPic(
        [FromForm] UpdateAccountPictureDto dto
    )
    {
        return Ok(await accountService.UpdateBackgroundPicture(dto));

    }
}