using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PictureCommentService : IPictureCommentService
{
    private readonly ICommentRepo _commentRepo;
    private readonly IPictureRepo _pictureRepo;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;
    private readonly IMapper _mapper;

    public PictureCommentService(
        ICommentRepo commentRepo,
        IPictureRepo pictureRepo,
        IAuthorizationService authorizationService,
        IAccountContextService accountContextService,
        IMapper mapper)
    {
        _commentRepo = commentRepo;
        _pictureRepo = pictureRepo;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
        _mapper = mapper;
    }
    public async Task<CommentDto> Create(int picId, string text)
    {
        if ((await _pictureRepo.GetByIdAsync(picId)) is null) throw new NotFoundException();
        var comment = new Comment()
        {
            Account = await _accountContextService.GetAccountAsync(),
            Picture = await _pictureRepo.GetByIdAsync(picId) ?? throw new NotFoundException(),
            Text = text
        };
        await _commentRepo.InsertAsync(comment);
        var result = _mapper.Map<CommentDto>(comment);
        result.IsModifiable = true;
        return result;
    }
    public async Task<CommentDto> Update(int commId, string text)
    {
        await AuthorizeCommentOperation(commId, ResourceOperation.Update ,"you cant modify a comment you didnt added");

        var comment = await _commentRepo.GetByIdAsync(commId) ?? throw new NotFoundException();
        comment.Text = text;
        await _commentRepo.UpdateAsync(comment);

        var result = _mapper.Map<CommentDto>(comment);
        result.IsModifiable = true;
        return result;
    }
    public async Task<bool> Delete(int commId)
    {
        await AuthorizeCommentOperation(commId, ResourceOperation.Delete,"you have no rights to delete this comment");
        return await _commentRepo.TryDeleteByIdAsync(commId);
    }

    private async Task AuthorizeCommentOperation(int commId, ResourceOperation operation, string message)
    {
        var comment = await _commentRepo.GetByIdAsync(commId) ?? throw new NotFoundException();

        if (comment is null) throw new NotFoundException();
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, comment, new CommentOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }
    
}