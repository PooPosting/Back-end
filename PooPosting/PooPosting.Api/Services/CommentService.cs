using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using PooPosting.Api.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Enums;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Comment;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Repos.Interfaces;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly ICommentRepo _commentRepo;
    private readonly IPictureRepo _pictureRepo;
    private readonly IAccountContextService _accountContextService;
    private readonly IAuthorizationService _authorizationService;

    public CommentService(
        IMapper mapper,
        ICommentRepo commentRepo,
        IPictureRepo pictureRepo,
        IAccountContextService accountContextService,
        IAuthorizationService authorizationService
        )
    {
        _mapper = mapper;
        _commentRepo = commentRepo;
        _pictureRepo = pictureRepo;
        _accountContextService = accountContextService;
        _authorizationService = authorizationService;
    }

    public async Task<PagedResult<CommentDto>> GetByPictureId(
        int picId,
        Query query
    )
    {
        var comments = await _commentRepo
            .GetByPictureIdAsync(
                picId,
                query.PageSize * (query.PageNumber - 1),
                query.PageSize);

        return new PagedResult<CommentDto>(
            _mapper.Map<IEnumerable<CommentDto>>(comments),
            query.PageNumber,
            query.PageSize,
            await _commentRepo.CountCommentsAsync(c => c.PictureId == picId)
        );
    }

    public async Task<CommentDto> Create(
        int picId,
        string text
        )
    {
        if ((await _pictureRepo.GetByIdAsync(picId)) is null) throw new NotFoundException();
        var comment = new Comment()
        {
            AccountId = _accountContextService.GetAccountId(),
            PictureId = picId,
            Text = text
        };
        var result = _mapper.Map<CommentDto>(await _commentRepo.InsertAsync(comment));
        result.IsModifiable = true;
        return result;
    }

    public async Task<CommentDto> Update(
        int commId,
        string text
        )
    {
        await AuthorizeCommentOperation(commId, ResourceOperation.Update ,"you cant modify a comment you didnt added");

        var comment = await _commentRepo.GetByIdAsync(commId) ?? throw new NotFoundException();
        comment.Text = text;
        await _commentRepo.UpdateAsync(comment);

        var result = _mapper.Map<CommentDto>(comment);
        result.IsModifiable = true;
        return result;
    }

    public async Task<bool> Delete(
        int commId
        )
    {
        await AuthorizeCommentOperation(commId, ResourceOperation.Delete,"you have no rights to delete this comment");
        var comment = await _commentRepo.GetByIdAsync(commId);
        if (comment is null) throw new NotFoundException();
        comment.IsDeleted = true;
        await _commentRepo.UpdateAsync(comment);
        return true;
    }

    private async Task AuthorizeCommentOperation(
        int commId,
        ResourceOperation operation,
        string message
        )
    {
        var comment = await _commentRepo.GetByIdAsync(commId) ?? throw new NotFoundException();

        if (comment is null) throw new NotFoundException();
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, comment, new CommentOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }
}