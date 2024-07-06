using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Queries;
using PooPosting.Application.Authorization;
using PooPosting.Application.Mappers;
using PooPosting.Application.Models;
using PooPosting.Application.Models.Dtos.Picture;
using PooPosting.Application.Models.Queries;
using PooPosting.Application.Services.Helpers;
using PooPosting.Application.Services.Interfaces;
using PooPosting.Domain.DbContext;
using PooPosting.Domain.DbContext.Entities;
using PooPosting.Domain.DbContext.Entities.Joins;
using PooPosting.Domain.Enums;
using PooPosting.Domain.Exceptions;

namespace PooPosting.Application.Services;

public class PictureService(
        ILogger<PictureService> logger,
        IAuthorizationService authorizationService,
        IAccountContextService accountContextService,
        PictureDbContext dbContext,
        IStorageService storageService
        )
    : IPictureService
{
    public async Task<PictureDto> GetById(int id)
    {
        var currAccId = accountContextService.TryGetAccountId();
        var picture = await dbContext.Pictures
            .Where(p => p.Id == id)
            .ProjectToDto(currAccId)
            .FirstOrDefaultAsync();
        
        return picture ?? throw new NotFoundException();
    }

    public async Task<PagedResult<PictureDto>> GetAll(Query query)
    {
        var currAccId = accountContextService.TryGetAccountId();
        
        var pictureDtos = dbContext.Pictures
            // .Where(p => p.SeenByAccount.All(sa => sa.AccountId != currAccId))
            .OrderByDescending(p => p.PopularityScore)
            .ThenByDescending(p => p.Id)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ProjectToDto(currAccId);

        if (currAccId is not null)
        {
            foreach (var picture in pictureDtos)
            {
                await MarkAsSeenAsync(currAccId.Value, IdHasher.DecodePictureId(picture.Id));
            }
            await dbContext.SaveChangesAsync();
        }
        
        return new PagedResult<PictureDto>(
            await pictureDtos.ToListAsync(),
            query.PageNumber,
            query.PageSize,
            await dbContext.Pictures.CountAsync()
        );
    }

    public async Task<PagedResult<PictureDto>> GetAll(SearchQuery query)
    {
        var picQuery = dbContext.Pictures
            .Where(p => 
                query.SearchPhrase != null && 
                (query.SearchPhrase == string.Empty || p.Description.Contains(query.SearchPhrase, StringComparison.CurrentCultureIgnoreCase))
                );

        switch (query.OrderBy)
        {
            case OrderBy.Newest:
                picQuery = picQuery.OrderByDescending(p => p.PictureAdded.Ticks);
                break;
            case OrderBy.MostLikes:
                picQuery = picQuery.OrderByDescending(p => p.Likes.Count);
                break;
            case OrderBy.MostPopular:
            default:
                picQuery = picQuery.OrderByDescending(p => p.PopularityScore);
                break;
        }

        var totalCount = await picQuery.CountAsync();
        var pictureDtos = await picQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ProjectToDto(accountContextService.TryGetAccountId())
            .ToListAsync();
        
        var result = new PagedResult<PictureDto>(
            pictureDtos,
            query.PageNumber,
            query.PageSize,
            totalCount
        );

        return result;
    }
    
    public async Task<PagedResult<PictureDto>> GetTrending(Query query)
    {
        // todo: maybe update this
        var currAccId = accountContextService.TryGetAccountId();

        var pictureDtos = dbContext.Pictures
            .OrderByDescending(p => p.Likes.Count)
            .Where(p => p.PictureAdded.AddDays(3) < DateTime.UtcNow)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ProjectToDto(currAccId);
        
        return new PagedResult<PictureDto>(
            await pictureDtos.ToListAsync(),
            query.PageNumber,
            query.PageSize,
            await dbContext.Pictures.CountAsync()
        );
    }
    
    public async Task<PictureDto> UpdateName(int picId, UpdatePictureNameDto dto)
    {
        var picture = await dbContext.Pictures
            .Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id == picId);
        if (picture == null) throw new NotFoundException();
        
        await AuthorizePictureOperation(picture, ResourceOperation.Update, "you cannot modify picture you didnt post");
        
        var result = dbContext.Pictures.Update(picture).Entity.MapToDto(accountContextService.TryGetAccountId());
        await dbContext.SaveChangesAsync();
        return result;
    }
    
    public async Task<PictureDto> UpdateDescription(int picId, UpdatePictureDescriptionDto dto)
    {
        var picture = await dbContext.Pictures
            .Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id == picId);
        if (picture == null) throw new NotFoundException();
        await AuthorizePictureOperation(picture, ResourceOperation.Update, "you cannot modify picture you didnt post");
        picture.Description = dto.Description;
        
        var result = dbContext.Pictures.Update(picture).Entity.MapToDto(accountContextService.TryGetAccountId());
        await dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<PictureDto> UpdateTags(int picId, UpdatePictureTagsDto dto)
    {
        var picture = await dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == picId);

        if (picture == null) throw new NotFoundException();

        await AuthorizePictureOperation(picture, ResourceOperation.Update, "You cannot modify a picture you didn't post");

        dbContext.PictureTags.RemoveRange(picture.PictureTags);

        var tagsToAdd = dto.Tags.Distinct().ToList();

        foreach (var tagValue in tagsToAdd)
        {
            var existingTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Value == tagValue);

            if (existingTag == null)
            {
                existingTag = new Tag { Value = tagValue };
                dbContext.Tags.Add(existingTag);
            }

            var pictureTag = new PictureTag { Picture = picture, Tag = existingTag };
            picture.PictureTags.Add(pictureTag);
        }

        await dbContext.SaveChangesAsync();

        var result = picture.MapToDto(accountContextService.TryGetAccountId());
        return result;
    }
    
    public async Task<string> Create(CreatePictureDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.DataUrl)) throw new BadRequestException("Invalid picture");

        var accountId = accountContextService.GetAccountId();
        
        var picture = new Picture
        {
            Description = dto.Description,
            AccountId = accountId,
            Url = await storageService.UploadFile(dto.DataUrl, $"pictures/{RandomName.Generate("webp")}"),
            PictureTags = await CreatePictureTags(dto),
        };

        await dbContext.Pictures.AddAsync(picture);
        await dbContext.SaveChangesAsync();

        return IdHasher.EncodePictureId(picture.Id);
    }

    private async Task<ICollection<PictureTag>> CreatePictureTags(CreatePictureDto dto)
    {
        if (dto.Tags == null || dto.Tags.Length == 0) return Array.Empty<PictureTag>();
        var existingTags = await dbContext.Tags
            .Where(tag => dto.Tags.Contains(tag.Value))
            .ToListAsync();

        var newTags = dto.Tags
            .Except(existingTags.Select(tag => tag.Value))
            .Select(tag => new Tag { Value = tag })
            .Where(t => t.Value != null!)
            .ToList();

        if (newTags.Any())
        {
            await dbContext.Tags.AddRangeAsync(newTags);
            await dbContext.SaveChangesAsync();
            existingTags.AddRange(newTags);
        }

        return existingTags.Select(tag => new PictureTag { TagId = tag.Id }).ToList();
    }

    public async Task<bool> Delete(int id)
    {
        var picture = await dbContext.Pictures
            .Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException();
        logger.LogWarning("Picture with id: {Id} DELETE (hide) action invoked", id);

        await AuthorizePictureOperation(picture, ResourceOperation.Delete ,"you have no rights to delete this picture");

        try
        {
            picture.IsDeleted = true;
            dbContext.Update(picture);
            await dbContext.SaveChangesAsync();
            logger.LogWarning("Picture with id: {Id} DELETE (hide) action success", id);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    

    #region Private methods

    private async Task AuthorizePictureOperation(Picture picture, ResourceOperation operation, string message)
    {
        var user = accountContextService.User;
        var authorizationResult = await authorizationService.AuthorizeAsync(user, picture, new PictureOperationRequirement(operation));
        if (!authorizationResult.Succeeded) throw new ForbidException(message);
    }
    
    private async Task MarkAsSeenAsync(int accountId, int pictureId)
    {
        if (!dbContext.PicturesSeenByAccounts.Any(j => (j.Account.Id == accountId) && (j.Picture.Id == pictureId)))
        {
            await dbContext.PicturesSeenByAccounts.AddAsync(new PictureSeenByAccount()
            {
                AccountId = accountId,
                PictureId = pictureId
            });
        }
    }

    #endregion
    
}