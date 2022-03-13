using System.Net.Http.Headers;
using Newtonsoft.Json;
using PicturesAPI.Models.HttpResponses;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services.Helpers;

public class ClassifyNsfw : IClassifyNsfw
{
    private readonly IConfiguration _config;
    public ClassifyNsfw(IConfiguration config)
    {
        _config = config;
    }
    public bool IsSafeForWork(string picId)
    {
        var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development",
            StringComparison.InvariantCultureIgnoreCase);
        
        if (isDevelopment) return true;
        
        var url = Path.Combine(_config.GetValue<string>("AppSecret:app-url"), $"wwwroot/pictures/{picId}.webp");
        
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_config.GetValue<string>("AppSecret:x-rapidapi-uri")),
            Headers =
            {
                { "x-rapidapi-host", _config.GetValue<string>("AppSecret:x-rapidapi-host") },
                { "x-rapidapi-key", _config.GetValue<string>("AppSecret:x-rapidapi-key") },
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