using PicturesAPI.Entities;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Services.Helpers.Interfaces;

namespace PicturesAPI.Services.Helpers;

public class PictureHelper : IPictureHelper
{
    private readonly PictureDbContext _dbContext;

    public PictureHelper(
        PictureDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }

    public async Task<bool> MarkAsSeenAsync(int accountId, int pictureId)
    {
        if (!_dbContext.PicturesSeenByAccounts
                .Any(j => (j.Account.Id == accountId) && (j.Picture.Id == pictureId)))
        {
            _dbContext.PicturesSeenByAccounts.Add(new PictureSeenByAccount()
            {
                AccountId = accountId,
                PictureId = pictureId
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return false;
    }

}