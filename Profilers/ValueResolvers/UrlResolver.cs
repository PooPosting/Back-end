using AutoMapper;
using PicturesAPI.Entities;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Profilers.ValueResolvers;

public class UrlResolver:
    IValueResolver<Picture, PictureDto, string>,
    IValueResolver<Picture, PicturePreviewDto, string>
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

    public string Resolve(Picture source, PicturePreviewDto destination, string destMember, ResolutionContext context)
    {
        return source.Url.StartsWith("http") ? source.Url : Path.Combine(_appOrigin, source.Url);
    }
}