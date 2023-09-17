using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Authorization;
using PooPosting.Api.Entities;
using PooPosting.Api.Entities.Joins;
using PooPosting.Api.Enums;
using PooPosting.Api.Exceptions;
using PooPosting.Api.Models;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Services;

public class PictureService : IPictureService
{
    private readonly ILogger<PictureService> _logger;
    private readonly IMapper _mapper;
    private readonly PictureDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAccountContextService _accountContextService;

    public PictureService(
        ILogger<PictureService> logger,
        IAuthorizationService authorizationService,
        IAccountContextService accountContextService,
        IMapper mapper,
        PictureDbContext dbContext
        )
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
        _authorizationService = authorizationService;
        _accountContextService = accountContextService;
    }

    public async Task<PictureDto> GetById(int id)
    {
        var picture = await _dbContext.Pictures
            .Where(p => p.Id == id)
            .ProjectTo<PictureDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return picture ?? throw new NotFoundException();
    }

    public async Task<IEnumerable<PictureDto>> GetAll(PersonalizedQuery query)
    {
        var accId = _accountContextService.GetAccountId();
        
        var picQueryable = _dbContext.Pictures
            .OrderByDescending(p => (p.PictureTags
                .Select(t => t.Tag)
                .SelectMany(t => t.AccountLikedTags)
                .Select(alt => alt.AccountId == accId).Count() + 1) * p.PopularityScore)
            .ThenByDescending(p => p.Id)
            .Where(p => !_dbContext.PicturesSeenByAccounts
                .Where(j => j.Account.Id == accId)
                .Any(j => j.Picture.Id == p.Id && j.Account.Id == accId))
            .Take(query.PageSize);

        var pictureDtos = _mapper.ProjectTo<PictureDto>(picQueryable).ToArray();

        foreach (var picture in pictureDtos)
        {
            await MarkAsSeenAsync(accId, IdHasher.DecodePictureId(picture.Id));
        }
        
        return pictureDtos;
    }

    public async Task<PagedResult<PictureDto>> GetAll(Query query)
    {
        var pictureDtos = _dbContext.Pictures
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .OrderByDescending(p => p.PopularityScore)
            .ThenByDescending(p => p.Id)
            .ProjectTo<PictureDto>(_mapper.ConfigurationProvider);
        
        return new PagedResult<PictureDto>(
            pictureDtos,
            query.PageNumber,
            query.PageSize,
            await _dbContext.Pictures.CountAsync()
        );
    }

    public async Task<PagedResult<PictureDto>> GetAll(CustomQuery query)
    {
        var picQuery = _dbContext.Pictures
            .Where(p => query.SearchPhrase == string.Empty || p.Name.ToLower().Contains(query.SearchPhrase.ToLower()));

        switch (query.SearchBy)
        {
            case SortBy.Newest:
                picQuery = picQuery.OrderByDescending(p => p.PictureAdded.Ticks);
                break;
            case SortBy.MostLikes:
                picQuery = picQuery.OrderByDescending(p => p.Likes.Count(l => l.IsLike));
                break;
            case SortBy.MostPopular:
            default:
                picQuery = picQuery.OrderByDescending(p => p.PopularityScore);
                break;
        }

        var totalCount = await picQuery.CountAsync();
        var pictures = await picQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToListAsync();

        var pictureDtos = _mapper.Map<List<PictureDto>>(pictures);
        var result = new PagedResult<PictureDto>(
            pictureDtos,
            query.PageNumber,
            query.PageSize,
            totalCount
        );

        return result;
    }
    
    public async Task<PictureDto> UpdateName(int picId, UpdatePictureNameDto dto)
    {
        var picture = await _dbContext.Pictures
            .Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id == picId);
        if (picture == null) throw new NotFoundException();
        await AuthorizePictureOperation(picture, ResourceOperation.Update, "you cannot modify picture you didnt post");
        picture.Name = dto.Name;
        var result = _mapper.Map<PictureDto>(_dbContext.Pictures.Update(picture).Entity);
        await _dbContext.SaveChangesAsync();
        return result;
    }
    
    public async Task<PictureDto> UpdateDescription(int picId, UpdatePictureDescriptionDto dto)
    {
        var picture = await _dbContext.Pictures
            .Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id == picId);
        if (picture == null) throw new NotFoundException();
        await AuthorizePictureOperation(picture, ResourceOperation.Update, "you cannot modify picture you didnt post");
        picture.Description = dto.Description;
        var result = _mapper.Map<PictureDto>(_dbContext.Pictures.Update(picture).Entity);
        await _dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<PictureDto> UpdateTags(int picId, UpdatePictureTagsDto dto)
    {
        var picture = await _dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == picId);

        if (picture == null) throw new NotFoundException();

        await AuthorizePictureOperation(picture, ResourceOperation.Update, "You cannot modify a picture you didn't post");

        _dbContext.PictureTags.RemoveRange(picture.PictureTags);

        var tagsToAdd = dto.Tags.Distinct().ToList();

        foreach (var tagValue in tagsToAdd)
        {
            var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Value == tagValue);

            if (existingTag == null)
            {
                existingTag = new Tag { Value = tagValue };
                _dbContext.Tags.Add(existingTag);
            }

            var pictureTag = new PictureTag { Picture = picture, Tag = existingTag };
            picture.PictureTags.Add(pictureTag);
        }

        await _dbContext.SaveChangesAsync();

        var result = _mapper.Map<PictureDto>(picture);
        return result;
    }
    
    public async Task<string> Create(CreatePictureDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FileBase64)) throw new BadRequestException("Invalid picture");

        var accountId = _accountContextService.GetAccountId();
        var picture = _mapper.Map<Picture>(dto);
        picture.AccountId = accountId;

        var rootPath = Directory.GetCurrentDirectory();
        var randomName = $"{Path.GetRandomFileName().Replace('.', '-')}.webp";
        var fullPath = Path.Combine(rootPath, "wwwroot", "pictures", randomName);
        picture.Url = Path.Combine("wwwroot", "pictures", randomName);

        try
        {
            var imageData = Convert.FromBase64String(dto.FileBase64);

            await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                await stream.WriteAsync(imageData, 0, imageData.Length);
            }

            if (dto.Tags is not null && dto.Tags.Any())
            {
                var existingTags = await _dbContext.Tags
                    .Where(tag => dto.Tags.Contains(tag.Value))
                    .ToListAsync();

                var newTags = dto.Tags
                    .Except(existingTags.Select(tag => tag.Value))
                    .Select(tag => new Tag { Value = tag })
                    .ToList();

                if (newTags.Any())
                {
                    await _dbContext.Tags.AddRangeAsync(newTags);
                    await _dbContext.SaveChangesAsync();
                    existingTags.AddRange(newTags);
                }

                picture.PictureTags = existingTags.Select(tag => new PictureTag { TagId = tag.Id }).ToList();
            }

            await _dbContext.Pictures.AddAsync(picture);
            await _dbContext.SaveChangesAsync();

            return IdHasher.EncodePictureId(picture.Id);
        }
        catch (Exception)
        {
            if (File.Exists(fullPath)) File.Delete(fullPath);
            throw;
        }
    }

    public async Task<bool> Delete(int id)
    {
        var picture = await _dbContext.Pictures
            .Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException();
        _logger.LogWarning("Picture with id: {Id} DELETE (hide) action invoked", id);

        await AuthorizePictureOperation(picture, ResourceOperation.Delete ,"you have no rights to delete this picture");

        try
        {
            picture.IsDeleted = true;
            _dbContext.Update(picture);
            await _dbContext.SaveChangesAsync();
            _logger.LogWarning("Picture with id: {Id} DELETE (hide) action success", id);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #region Private methods

    private async Task AuthorizePictureOperation(
        Picture picture,
        ResourceOperation operation,
        string message
        )
    {
        var user = _accountContextService.User;
        var authorizationResult = await _authorizationService.AuthorizeAsync(user, picture, new PictureOperationRequirement(operation));
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }
    
    private async Task MarkAsSeenAsync(int accountId, int pictureId)
    {
        if (!_dbContext.PicturesSeenByAccounts.Any(j => (j.Account.Id == accountId) && (j.Picture.Id == pictureId)))
        {
            await _dbContext.PicturesSeenByAccounts.AddAsync(new PictureSeenByAccount()
            {
                AccountId = accountId,
                PictureId = pictureId
            });
        }
    }

    #endregion
    
}