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

            if (item.AccountPreview.Id == accountId) item.IsModifiable = true;
            else if (accountRole == 3) item.IsAdminModifiable = true;

            if (item.Likes.Any())
            {
                foreach (var like in item.Likes)
                {
                    if (like.AccountPreview is not null)
                    {
                        if (like.AccountPreview.ProfilePicUrl is not null)
                            like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                                ? like.AccountPreview.ProfilePicUrl
                                : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                    }                        }
            }
            if (item.Comments.Any())
            {
                foreach (var comment in item.Comments)
                {
                    if (comment.AccountPreview is not null)
                    {
                        if (comment.AccountPreview.ProfilePicUrl is not null)
                            comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                                ? comment.AccountPreview.ProfilePicUrl
                                : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                        if (comment.AccountPreview.Id == accountId) comment.IsModifiable = true;
                        else if (accountRole == 3) item.IsAdminModifiable = true;
                    }
                }
            }

            if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && l.IsLike)))
            {
                item.LikeState = LikeState.Liked;
            }
            if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && !l.IsLike)))
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

        if (item.AccountPreview.Id == accountId) item.IsModifiable = true;
        else if (accountRole == 3) item.IsAdminModifiable = true;

        if (item.Likes.Any())
        {
            foreach (var like in item.Likes)
            {
                if (like.AccountPreview is not null)
                {
                    if (like.AccountPreview.ProfilePicUrl is not null)
                        like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                            ? like.AccountPreview.ProfilePicUrl
                            : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                }                        }
        }
        if (item.Comments.Any())
        {
            foreach (var comment in item.Comments)
            {
                if (comment.AccountPreview is not null)
                {
                    if (comment.AccountPreview.ProfilePicUrl is not null)
                        comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                            ? comment.AccountPreview.ProfilePicUrl
                            : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                    if (comment.AccountPreview.Id == accountId) comment.IsModifiable = true;
                    else if (accountRole == 3) item.IsAdminModifiable = true;
                }
            }
        }

        if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && l.IsLike)))
        {
            item.LikeState = LikeState.Liked;
        }
        if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && !l.IsLike)))
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

                if (item.AccountPreview.Id == accountId) item.IsModifiable = true;
                else if (accountRole == 3) item.IsAdminModifiable = true;

                if (item.Likes.Any())
                {
                    foreach (var like in item.Likes)
                    {
                        if (like.AccountPreview is not null)
                        {
                            if (like.AccountPreview.ProfilePicUrl is not null)
                                like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                                    ? like.AccountPreview.ProfilePicUrl
                                    : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                        }                        }
                }
                if (item.Comments.Any())
                {
                    foreach (var comment in item.Comments)
                    {
                        if (comment.AccountPreview is not null)
                        {
                            if (comment.AccountPreview.ProfilePicUrl is not null)
                                comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                                    ? comment.AccountPreview.ProfilePicUrl
                                    : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                            if (comment.AccountPreview.Id == accountId) comment.IsModifiable = true;
                            else if (accountRole == 3) item.IsAdminModifiable = true;
                        }
                    }
                }

                if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && l.IsLike)))
                {
                    item.LikeState = LikeState.Liked;
                }
                if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && !l.IsLike)))
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

            if (item.AccountPreview.Id == accountId) item.IsModifiable = true;
            else if (accountRole == 3) item.IsAdminModifiable = true;

            if (item.Likes.Any())
            {
                foreach (var like in item.Likes)
                {
                    if (like.AccountPreview is not null)
                    {
                        if (like.AccountPreview.ProfilePicUrl is not null)
                            like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                                ? like.AccountPreview.ProfilePicUrl
                                : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                    }                        }
            }
            if (item.Comments.Any())
            {
                foreach (var comment in item.Comments)
                {
                    if (comment.AccountPreview is not null)
                    {
                        if (comment.AccountPreview.ProfilePicUrl is not null)
                            comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                                ? comment.AccountPreview.ProfilePicUrl
                                : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                        if (comment.AccountPreview.Id == accountId) comment.IsModifiable = true;
                        else if (accountRole == 3) item.IsAdminModifiable = true;
                    }
                }
            }

            if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && l.IsLike)))
            {
                item.LikeState = LikeState.Liked;
            }
            if (item.Likes.Any(l => (l.AccountPreview.Id == accountId && !l.IsLike)))
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

            if (account.Pictures.Any())
            {
                foreach (var picture in account.Pictures)
                {
                    if (picture.AccountPreview.ProfilePicUrl is not null)
                        picture.AccountPreview.ProfilePicUrl = picture.AccountPreview.ProfilePicUrl.StartsWith("http") ? picture.AccountPreview.ProfilePicUrl : Path.Combine(_appOrigin, picture.AccountPreview.ProfilePicUrl);

                    picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);

                    if (picture.Likes.Any())
                    {
                        foreach (var like in picture.Likes)
                        {
                            if (like.AccountPreview is not null)
                            {
                                if (like.AccountPreview.ProfilePicUrl is not null)
                                    like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                                        ? like.AccountPreview.ProfilePicUrl
                                        : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                            }                        }
                    }
                    if (picture.Comments.Any())
                    {
                        foreach (var comment in picture.Comments)
                        {
                            if (comment.AccountPreview is not null)
                            {
                                if (comment.AccountPreview.ProfilePicUrl is not null)
                                    comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                                        ? comment.AccountPreview.ProfilePicUrl
                                        : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                            }                       }
                    }
                }
            }
        }
    }
    private void AdjustUrls(IEnumerable<PictureDto> pictures)
    {
        foreach (var picture in pictures)
        {
            if (picture.AccountPreview is not null)
            {
                if (picture.AccountPreview.ProfilePicUrl is not null)
                    picture.AccountPreview.ProfilePicUrl = picture.AccountPreview.ProfilePicUrl.StartsWith("http") ? picture.AccountPreview.ProfilePicUrl : Path.Combine(_appOrigin, picture.AccountPreview.ProfilePicUrl);
            }
            picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);

            if (picture.Likes.Any())
            {
                foreach (var like in picture.Likes)
                {
                    if (like.AccountPreview is not null)
                    {
                        if (like.AccountPreview.ProfilePicUrl is not null)
                            like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                                ? like.AccountPreview.ProfilePicUrl
                                : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                    }
                }
            }
            if (picture.Comments.Any())
            {
                foreach (var comment in picture.Comments)
                {
                    if (comment.AccountPreview is not null)
                    {
                        if (comment.AccountPreview.ProfilePicUrl is not null)
                            comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                                ? comment.AccountPreview.ProfilePicUrl
                                : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                    }
                }
            }
        }
    }
    private void AdjustUrls(AccountDto account)
    {
        if (account.BackgroundPicUrl is not null)
            account.BackgroundPicUrl = account.BackgroundPicUrl.StartsWith("http") ? account.BackgroundPicUrl : Path.Combine(_appOrigin, account.BackgroundPicUrl);
        if (account.ProfilePicUrl is not null)
            account.ProfilePicUrl = account.ProfilePicUrl.StartsWith("http") ? account.ProfilePicUrl : Path.Combine(_appOrigin, account.ProfilePicUrl);

        if (account.Pictures.Any())
        {
            foreach (var picture in account.Pictures)
            {
                if (picture.AccountPreview.ProfilePicUrl is not null)
                    picture.AccountPreview.ProfilePicUrl = picture.AccountPreview.ProfilePicUrl.StartsWith("http") ? picture.AccountPreview.ProfilePicUrl : Path.Combine(_appOrigin, picture.AccountPreview.ProfilePicUrl);

                picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);

                if (picture.Likes.Any())
                {
                    foreach (var like in picture.Likes)
                    {
                        if (like.AccountPreview is not null)
                        {
                            if (like.AccountPreview.ProfilePicUrl is not null)
                                like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                                    ? like.AccountPreview.ProfilePicUrl
                                    : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                        }                   }
                }
                if (picture.Comments.Any())
                {
                    foreach (var comment in picture.Comments)
                    {
                        if (comment.AccountPreview is not null)
                        {
                            if (comment.AccountPreview.ProfilePicUrl is not null)
                                comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                                    ? comment.AccountPreview.ProfilePicUrl
                                    : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                        }
                    }
                }
            }
        }
    }
    private void AdjustUrls(PictureDto picture)
    {
        if (picture.AccountPreview is not null)
        {
            if (picture.AccountPreview.ProfilePicUrl is not null)
                picture.AccountPreview.ProfilePicUrl = picture.AccountPreview.ProfilePicUrl.StartsWith("http") ? picture.AccountPreview.ProfilePicUrl : Path.Combine(_appOrigin, picture.AccountPreview.ProfilePicUrl);
        }

        picture.Url = picture.Url.StartsWith("http") ? picture.Url : Path.Combine(_appOrigin, picture.Url);
        if (picture.Likes.Any())
        {
            foreach (var like in picture.Likes)
            {
                if (like.AccountPreview is not null)
                {
                    if (like.AccountPreview.ProfilePicUrl is not null)
                        like.AccountPreview.ProfilePicUrl = like.AccountPreview.ProfilePicUrl.StartsWith("http")
                            ? like.AccountPreview.ProfilePicUrl
                            : Path.Combine(_appOrigin, like.AccountPreview.ProfilePicUrl);
                }
            }
        }
        if (picture.Comments.Any())
        {
            foreach (var comment in picture.Comments)
            {
                if (comment.AccountPreview is not null)
                {
                    if (comment.AccountPreview.ProfilePicUrl is not null)
                        comment.AccountPreview.ProfilePicUrl = comment.AccountPreview.ProfilePicUrl.StartsWith("http")
                            ? comment.AccountPreview.ProfilePicUrl
                            : Path.Combine(_appOrigin, comment.AccountPreview.ProfilePicUrl);
                }
            }
        }
    }

    #endregion
}