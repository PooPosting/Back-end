﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Application.Models.Dtos.Comment;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Domain.DbContext.Pagination;

namespace PooPosting.Api.Controllers.Comment;

[ApiController]
[Route("api/picture/{picId}/comment")]
public class PictureCommentsController(
    ICommentService commentService
    ) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetPictureComments(
        [FromRoute] string picId,
        [FromQuery] PaginationParameters paginationParameters
        )
    {
        var result = await commentService.GetByPictureId(IdHasher.DecodePictureId(picId), paginationParameters);
        return Ok(result);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostComment(
        [FromRoute] string picId,
        [FromBody] PostPutCommentDto dto
        )
    {
        var result = await commentService.Create(IdHasher.DecodePictureId(picId), dto.Text);
        return Ok(result);
    }


}