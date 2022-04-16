using System.Net.Http.Headers;
using Newtonsoft.Json;
using PicturesAPI.Configuration;
using PicturesAPI.Models.HttpResponses;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services.Helpers;

public class ClassifyNsfw : IClassifyNsfw
{
    private readonly RapidApiSettings _rapidApiSettings;
    public ClassifyNsfw(IConfiguration config)
    {
        _rapidApiSettings = new RapidApiSettings()
        {
            AppUrl = config.GetValue<string>("RapidApiSettings:AppUrl"),
            RapidApiHost = config.GetValue<string>("RapidApiSettings:RapidApiHost"),
            RapidApiKey = config.GetValue<string>("RapidApiSettings:RapidApiKey"),
            RapidApiUri = config.GetValue<string>("RapidApiSettings:RapidApiUri"),
        };
    }
    public bool IsSafeForWork(string picId)
    {
        var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development",
            StringComparison.InvariantCultureIgnoreCase);
        
        if (isDevelopment) return true;
        var url = Path.Combine(_rapidApiSettings.AppUrl, $"wwwroot/pictures/{picId}.webp");
        
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_rapidApiSettings.RapidApiUri),
            Headers =
            {
                { "x-rapidapi-host", _rapidApiSettings.RapidApiHost },
                { "x-rapidapi-key", _rapidApiSettings.RapidApiKey },
            },
            Content = new StringContent("{\r\"url\": \"" + url + "\"\r}")
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        
        using (var response = client.Send(request))
        {
            response.EnsureSuccessStatusCode();
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<ClassifyNsfwModel>(body);
            return !result?.Unsafe ?? false;
        }

    }
}