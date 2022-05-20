using Microsoft.AspNetCore.Mvc.Filters;
using PicturesAPI.Exceptions;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.ActionFilters;

public class IsIpBannedFilter: ActionFilterAttribute
{
    private readonly IRestrictedIpRepo _restrictedIpRepo;

    public IsIpBannedFilter(
        IRestrictedIpRepo restrictedIpRepo)
    {
        _restrictedIpRepo = restrictedIpRepo;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress!.ToString();
        var restrictedIp = _restrictedIpRepo.GetRestrictedIp(remoteIp);
        if (restrictedIp is null) return;
        if (restrictedIp.Banned) throw new ForbidException("Your ip is banned");
    }
}