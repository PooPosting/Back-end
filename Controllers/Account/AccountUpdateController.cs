using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PicturesAPI.Models.Dtos.Account;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers.Account;

[ApiController]
[Authorize]
[Route("api/account/update")]
public class AccountUpdateController: ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountUpdateController(
        IAccountService accountService
        )
    {
        _accountService = accountService;
    }

    [HttpPost]
    [Route("email")]
    public async Task<IActionResult> UpdateAccountEmail(
        [FromBody] UpdateAccountEmailDto dto
    )
    {
        return Ok(await _accountService.UpdateEmail(dto));
    }

    [HttpPost]
    [Route("password")]
    public async Task<IActionResult> UpdateAccountPassword(
        [FromBody] UpdateAccountPasswordDto dto
    )
    {
        return Ok(await _accountService.UpdatePassword(dto));
    }

    [HttpPatch]
    [Route("description")]
    public async Task<IActionResult> UpdateAccountDescription(
        [FromBody] UpdateAccountDescriptionDto dto
    )
    {
        return Ok(await _accountService.UpdateDescription(dto));
    }

    [HttpPatch]
    [Route("profile-picture")]
    public async Task<IActionResult> UpdateAccountProfilePic(
        [FromForm] UpdateAccountPictureDto dto
    )
    {
        return Ok(await _accountService.UpdateProfilePicture(dto));

    }

    [HttpPatch]
    [Route("background-picture")]
    public async Task<IActionResult> UpdateAccountBackgroundPic(
        [FromForm] UpdateAccountPictureDto dto
    )
    {
        return Ok(await _accountService.UpdateBackgroundPicture(dto));

    }
}