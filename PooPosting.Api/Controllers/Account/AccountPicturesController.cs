#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Application.Models.Queries;
using PooPosting.Application.Services;
using PooPosting.Domain.DbContext.Interfaces;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Api.Controllers.Account;

[ApiController]
[Route("api/account/{accId}/picture")]
public class AccountPicturesController(AccountPicturesService accountPicService) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetAccountPictures(
        [FromQuery] AccountQueryParams paginationParameters,
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
        [FromQuery] PictureQueryParams paginationParameters,
        [FromRoute] string accId
    )
    {
        var accounts = await accountPicService.GetLikedPaged(paginationParameters, accId);
        return Ok(accounts);
    }
}