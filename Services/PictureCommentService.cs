using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using PicturesAPI.Authorization;
using PicturesAPI.Enums;
using PicturesAPI.Exceptions;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services;

public class PictureCommentService : IPictureCommentService
{
    private readonly ICommentRepo _commentRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IPictureRepo _pictureRepo;
    private readonly IPictureService _pictureService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;
    private readonly IMapper _mapper;

    public PictureCommentService(
        ICommentRepo commentRepo,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        IPictureService pictureService,
        IAuthorizationService authorizationService,
        IAccountContextService accountContextService,
        IMapper mapper)
    {
        _commentRepo = commentRepo;
        _accountRepo = accountRepo;
        _pictureRepo = pictureRepo;
        _pictureService = pictureService;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
        _mapper = mapper;
    }
    public CommentDto CreateComment(Guid picId, string text)
    {
        var accountId = _accountContextService.GetAccountId;
        if (accountId is null) throw new ForbidException("please log in");
        if (!_accountRepo.Exists(Guid.Parse(accountId))) throw new InvalidAuthTokenException();
        if (!_pictureRepo.Exists(picId)) throw new NotFoundException("picture not found");

        var id = _commentRepo.CreateComment(picId, Guid.Parse(accountId), text);
        var comment = _commentRepo.GetComment(id);
        var result = _mapper.Map<CommentDto>(comment);
        result.IsModifiable = true;
        
        return result;
    }
    public CommentDto ModifyComment(Guid commId, string text)
    {
        AuthorizeCommentOperation(commId, ResourceOperation.Update ,"you cant modify a picture you didnt added");
        
        _commentRepo.ModifyComment(commId, text);
        var comment = _commentRepo.GetComment(commId);
        var result = _mapper.Map<CommentDto>(comment);
        result.IsModifiable = true;
        
        return result;
    }
    public bool DeleteComment(Guid commId)
    {
        AuthorizeCommentOperation(commId, ResourceOperation.Delete,"you have no rights to delete this comment");
        
        var success = _commentRepo.DeleteComment(commId);
        return success;
    }

    private void AuthorizeCommentOperation(Guid commId, ResourceOperation operation, string message)
    {
        var comment = _commentRepo.GetComment(commId);
        if (comment is null) throw new NotFoundException("comment not found");
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, comment, new CommentOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }
    
}