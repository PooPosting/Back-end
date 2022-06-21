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
    private readonly IAccountRepo _accountRepo;
    private readonly IPictureRepo _pictureRepo;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;
    private readonly IMapper _mapper;

    public PictureCommentService(
        ICommentRepo commentRepo,
        IAccountRepo accountRepo,
        IPictureRepo pictureRepo,
        IAuthorizationService authorizationService,
        IAccountContextService accountContextService,
        IMapper mapper)
    {
        _commentRepo = commentRepo;
        _accountRepo = accountRepo;
        _pictureRepo = pictureRepo;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
        _mapper = mapper;
    }
    public CommentDto Create(int picId, string text)
    {
        var accountId = _accountContextService.GetAccountId();
        if ((_pictureRepo.GetById(picId)) is null) throw new NotFoundException("picture not found");

        var comment = new Comment()
        {
            Account = _accountRepo.GetById(accountId),
            Picture = _pictureRepo.GetById(picId),
            Text = text
        };
        _commentRepo.Insert(comment);
        _commentRepo.Save();
        var result = _mapper.Map<CommentDto>(comment);
        result.IsModifiable = true;
        return result;
    }
    public CommentDto Update(int commId, string text)
    {
        AuthorizeCommentOperation(commId, ResourceOperation.Update ,"you cant modify a comment you didnt added");

        var comment = _commentRepo.GetById(commId);
        comment.Text = text;
        _commentRepo.Update(comment);
        _commentRepo.Save();

        var result = _mapper.Map<CommentDto>(comment);
        result.IsModifiable = true;
        return result;
    }
    public void Delete(int commId)
    {
        AuthorizeCommentOperation(commId, ResourceOperation.Delete,"you have no rights to delete this comment");
        _commentRepo.DeleteById(commId);
    }

    // change this
    private void AuthorizeCommentOperation(int commId, ResourceOperation operation, string message)
    {
        var comment = _commentRepo.GetById(commId);
        if (comment is null) throw new NotFoundException("comment not found");
        var user = _accountContextService.User;
        var authorizationResult = _authorizationService.AuthorizeAsync(user, comment, new CommentOperationRequirement(operation)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }
    
}