using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PicturesAPI.Authorization;
using PicturesAPI.Entities;
using PicturesAPI.Exceptions;
using PicturesAPI.Interfaces;
using PicturesAPI.Models;

namespace PicturesAPI.Services;

public class PictureService : IPictureService
{
    private readonly ILogger<PictureService> _logger;
    private readonly PictureDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;

    public PictureService(ILogger<PictureService> logger, PictureDbContext dbContext, IMapper mapper,
        IAuthorizationService authorizationService, IAccountContextService accountContextService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
    }

    public IEnumerable<PictureDto> GetAllPictures()
    {
        var pictures = _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .ToList();

        if (pictures.Count == 0) throw new NotFoundException("pictures not found");
        var result = _mapper.Map<List<PictureDto>>(pictures).ToList();
        return result;
    }
        
    public PictureDto GetSinglePictureById(Guid id)
    {
        var picture = _dbContext.Pictures
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .SingleOrDefault(p => p.Id == id);

        if (picture == null) throw new NotFoundException("picture not found");
        var result = _mapper.Map<PictureDto>(picture);
        return result;
    }

    public Guid CreatePicture(CreatePictureDto dto)
    {
        var id = _accountContextService.GetAccountId;
        if (id is null) throw new InvalidAuthTokenException();
        
        dto.Tags = dto.Tags.Distinct().ToList();
        
        var picture = _mapper.Map<Picture>(dto);

        picture.PictureAdded = DateTime.Now;
        picture.AccountId = Guid.Parse(id);

        _dbContext.Add(picture);
        _dbContext.SaveChanges();
        return picture.Id;
    }

    public void PutPicture(Guid id, PutPictureDto dto)
    {
        var picture = _dbContext.Pictures.SingleOrDefault(p => p.Id == id);
        if (picture is null) throw new NotFoundException("There's not such a picture with that ID");
        var user = _accountContextService.User;

        var authorizationResult = _authorizationService.AuthorizeAsync(user, picture, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException("You can't modify picture you didn't added");

        dto.Tags = dto.Tags.Distinct().ToList();
        
        if (dto.Description != null) picture!.Description = dto.Description;
        if (dto.Name != null) picture!.Name = dto.Name;
        if (dto.Url != null) picture!.Url = dto.Url;
        if (dto.Tags != null) picture!.Tags = string.Join(" ", dto.Tags);
        _dbContext.SaveChanges();
    }
        
    public void DeletePicture(Guid id)
    {
        _logger.LogWarning($"Picture with id: {id} DELETE action invoked");
        var user = _accountContextService.User;
        var picture = _dbContext.Pictures.SingleOrDefault(p => p.Id == id);
        if (picture is null)
        {
            _logger.LogWarning($"Picture with id: {id} DELETE action failed (not found)");
            throw new NotFoundException("There's not such a picture with that ID");
        }
            
        var authorizationResult = _authorizationService.AuthorizeAsync(user, picture,
            new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
        if (!authorizationResult.Succeeded) throw new ForbidException("You can't delete picture you didn't added");

        var likesToRemove = _dbContext.Likes.Where(l => l.Liked == picture);
        var disLikesToRemove = _dbContext.Dislikes.Where(d => d.DisLiked == picture);
        
        _dbContext.Likes.RemoveRange(likesToRemove);
        _dbContext.Dislikes.RemoveRange(disLikesToRemove);
        _dbContext.Pictures.Remove(picture);
        _dbContext.SaveChanges();
        _logger.LogWarning($"Picture with id: {id} DELETE action success");

    }

}