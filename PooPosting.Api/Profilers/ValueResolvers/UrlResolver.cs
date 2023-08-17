using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Picture;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Profilers.ValueResolvers;

public class UrlResolver:
    IValueResolver<Picture, PictureDto, string>
{
    private readonly string _appOrigin;
    public UrlResolver(IConfiguration configuration)
    {
        _appOrigin = configuration.GetValue<string>("AppSettings:Origin");
    }

    public string Resolve(Picture source, PictureDto destination, string destMember, ResolutionContext context)
    {
        return source.Url.StartsWith("http") ? source.Url : Path.Combine(_appOrigin, source.Url);
    }

}