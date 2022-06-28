using PicturesAPI.Enums;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Helpers.Interfaces;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services.Helpers;

public class ModifyAllower : IModifyAllower
{
    private readonly IAccountContextService _accountContextService;
    private readonly string _appOrigin;

    public ModifyAllower(
        IAccountContextService accountContextService,
        IConfiguration configuration)
    {
        _accountContextService = accountContextService;
        _appOrigin = configuration.GetValue<string>("AppSettings:Origin");
    }

    public void UpdateItems(IEnumerable<PictureDto> items)
    {
        if (_accountContextService.TryGetAccountId() is null)
        {
            AdjustUrls(items);
            return;
        };

        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();
        foreach (var item in items)
        {
            item.Url = item.Url.StartsWith("http") ? item.Url : Path.Combine(_appOrigin, item.Url);

            if (item.AccountId == accountId) item.IsModifiable = true;
            else if (accountRole == 3) item.IsAdminModifiable = true;

            foreach (var comment in item.Comments)
            {
                if (comment.AccountId == accountId) comment.IsModifiable = true;
                else if (accountRole == 3) item.IsAdminModifiable = true;
            }

            if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
            {
                item.LikeState = LikeState.Liked;
            }
            if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
            {
                item.LikeState = LikeState.DisLiked;
            }
        }
    }
    public void UpdateItems(PictureDto item)
    {
        if (_accountContextService.TryGetAccountId() is null)
        {
            AdjustUrls(item);
            return;
        };
        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();

        item.Url = item.Url.StartsWith("http") ? item.Url : Path.Combine(_appOrigin, item.Url);

        if (item.AccountId == accountId) item.IsModifiable = true;
        else if (accountRole == 3) item.IsAdminModifiable = true;

        foreach (var comment in item.Comments)
        {
            if (comment.AccountId == accountId) comment.IsModifiable = true;
            else if (accountRole == 3) item.IsAdminModifiable = true;
        }

        if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
        {
            item.LikeState = LikeState.Liked;
        }
        if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
        {
            item.LikeState = LikeState.DisLiked;
        }
    }
    public void UpdateItems(IEnumerable<AccountDto> accounts)
    {
        if (_accountContextService.TryGetAccountId() is null)
        {
            AdjustUrls(accounts);
            return;
        };

        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();
        foreach (var account in accounts)
        {
            if (accountId == account.Id) account.IsModifiable = true;
            if (accountRole == 3) account.IsAdminModifiable = true;

            if (account.BackgroundPicUrl is not null)
                account.BackgroundPicUrl = account.BackgroundPicUrl.StartsWith("http") ? account.BackgroundPicUrl : Path.Combine(_appOrigin, account.BackgroundPicUrl);
            if (account.ProfilePicUrl is not null)
                account.ProfilePicUrl = account.ProfilePicUrl.StartsWith("http") ? account.ProfilePicUrl : Path.Combine(_appOrigin, account.ProfilePicUrl);

            foreach (var item in account.Pictures)
            {
                item.Url = item.Url.StartsWith("http") ? item.Url : Path.Combine(_appOrigin, item.Url);

                if (item.AccountId == accountId) item.IsModifiable = true;
                else if (accountRole == 3) item.IsAdminModifiable = true;

                foreach (var comment in item.Comments)
                {
                    if (comment.AccountId == accountId) comment.IsModifiable = true;
                    else if (accountRole == 3) item.IsAdminModifiable = true;
                }

                if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
                {
                    item.LikeState = LikeState.Liked;
                }
                if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
                {
                    item.LikeState = LikeState.DisLiked;
                }
            }
        }
    }
    public void UpdateItems(AccountDto account)
    {
        if (_accountContextService.TryGetAccountId() is null)
        {
            AdjustUrls(account);
            return;
        };

        var accountRole = _accountContextService.GetAccountRole();
        var accountId = _accountContextService.GetEncodedAccountId();

        if (account.BackgroundPicUrl is not null)
            account.BackgroundPicUrl = account.BackgroundPicUrl.StartsWith("http") ? account.BackgroundPicUrl : Path.Combine(_appOrigin, account.BackgroundPicUrl);
        if (account.ProfilePicUrl is not null)
            account.ProfilePicUrl = account.ProfilePicUrl.StartsWith("http") ? account.ProfilePicUrl : Path.Combine(_appOrigin, account.ProfilePicUrl);

        if (accountId == account.Id) account.IsModifiable = true;
        if (accountRole == 3) account.IsAdminModifiable = true;

        foreach (var item in account.Pictures)
        {
            item.Url = item.Url.StartsWith("http") ? item.Url : Path.Combine(_appOrigin, item.Url);

            if (item.AccountId == accountId) item.IsModifiable = true;
            else if (accountRole == 3) item.IsAdminModifiable = true;

            foreach (var comment in item.Comments)
            {
                if (comment.AccountId == accountId) comment.IsModifiable = true;
                else if (accountRole == 3) item.IsAdminModifiable = true;
            }

            if (item.Likes.Any(l => (l.AccountId == accountId && l.IsLike)))
            {
                item.LikeState = LikeState.Liked;
            }
            if (item.Likes.Any(l => (l.AccountId == accountId && !l.IsLike)))
            {
                item.LikeState = LikeState.DisLiked;
            }
        }
    }



    #region Private methods

    private void AdjustUrls(IEnumerable<AccountDto> accounts)
    {
        foreach (var account in accounts)
        {
            if (account.BackgroundPicUrl is not null)
                account.BackgroundPicUrl = account.BackgroundPicUrl.StartsWith("http") ? account.BackgroundPicUrl : Path.Combine(_appOrigin, account.BackgroundPicUrl);
            if (account.ProfilePicUrl is not null)
                account.ProfilePicUrl = account.ProfilePicUrl.StartsWith("http") ? account.ProfilePicUrl : Path.Combine(_appOrigin, account.ProfilePicUrl);

            foreach (var picture in account.Pictures)
            {
                picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);
            }
        }
    }
    private void AdjustUrls(IEnumerable<PictureDto> pictures)
    {
        foreach (var picture in pictures)
        {
            picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);
        }
    }
    private void AdjustUrls(AccountDto account)
    {
        if (account.BackgroundPicUrl is not null)
            account.BackgroundPicUrl = account.BackgroundPicUrl.StartsWith("http") ? account.BackgroundPicUrl : Path.Combine(_appOrigin, account.BackgroundPicUrl);
        if (account.ProfilePicUrl is not null)
            account.ProfilePicUrl = account.ProfilePicUrl.StartsWith("http") ? account.ProfilePicUrl : Path.Combine(_appOrigin, account.ProfilePicUrl);

        foreach (var picture in account.Pictures)
        {
            picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);
        }
    }
    private void AdjustUrls(PictureDto picture)
    {
        picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);
    }

    #endregion
}