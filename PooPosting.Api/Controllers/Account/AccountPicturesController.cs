#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Data.DbContext.Pagination;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Api.Controllers.Account;

[ApiController]
[Route("api/account/{accId}/picture")]
public class AccountPicturesController(
    IAccountPicturesService accountPicService
    ) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetAccountPictures(
        [FromQuery] PaginationParameters paginationParameters,
        [FromRoute] string accId
    )
    {
        var pictures = await accountPicService.GetPaged(paginationParameters, accId);
        return Ok(pictures);
    }
    
    [HttpGet]
    [EnableQuery]
    [Route("liked")]
    public async Task<IActionResult> GetLikedPictures(
        [FromQuery] PaginationParameters paginationParameters,
        [FromRoute] string accId
    )
    {
        var accounts = await accountPicService.GetLikedPaged(paginationParameters, accId);
        return Ok(accounts);
    }
}