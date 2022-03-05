using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;

public class PictureRepo : IPictureRepo
{
    private readonly PictureDbContext _dbContext;
    private readonly ILikeRepo _likeRepo;

    public PictureRepo(
        PictureDbContext dbContext, 
        ILikeRepo likeRepo)
    {
        _dbContext = dbContext;
        _likeRepo = likeRepo;
    }

    public IEnumerable<Picture> GetPictures()
    {
        var pictures = _dbContext.Pictures
            .Include(p => p.Likes);

        return pictures;
    }

    public IEnumerable<Picture> GetPicturesByOwner(Account account)
    {
        var pictures = _dbContext.Pictures
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Account)
            .Where(p => p.Account == account);
        return pictures;
    }
    
    public Picture GetPictureById(Guid id)
    {
        var picture = _dbContext.Pictures
            .Include(p => p.Likes)
            .ThenInclude(l => l.Liker)
            .Include(p => p.Account)
            .SingleOrDefault(p => p.Id == id);
        return picture;
    }

    public Guid CreatePicture(Picture picture)
    {
        _dbContext.Pictures.Add(picture);
        _dbContext.SaveChanges();
        return picture.Id;
    }

    public bool UpdatePicture(Picture picture, PutPictureDto dto)
    {
        var pictureToUpdate = _dbContext.Pictures.SingleOrDefault(p => p == picture);
        
        dto.Tags = dto.Tags.Distinct().ToList();
        if (dto.Description != null) pictureToUpdate!.Description = dto.Description;
        if (dto.Name != null) pictureToUpdate!.Name = dto.Name;
        if (dto.Tags != null) pictureToUpdate!.Tags = string.Join(" ", dto.Tags).ToLower();
        _dbContext.SaveChanges();
        
        return true;
    }

    public bool DeletePicture(Picture picture)
    {
        var likesToRemove = _likeRepo.GetLikesByLiked(picture);
        if (likesToRemove is not null) _dbContext.Likes.RemoveRange(likesToRemove);
        
        var pictureToRemove = _dbContext.Pictures.SingleOrDefault(p => p == picture)!;
        
        _dbContext.Pictures.Remove(pictureToRemove);
        _dbContext.SaveChanges();
        return true;
    }
    
    
}