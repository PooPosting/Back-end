using System.Net.Http.Headers;
using Newtonsoft.Json;
using PicturesAPI.Models.HttpResponses;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services.Helpers;

public class ClassifyNsfw : IClassifyNsfw
{
    public bool IsSafeForWork(string picId)
    {
        var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development",
            StringComparison.InvariantCultureIgnoreCase);
        if (isDevelopment) return true;
        
        //get root api path from appsettings.json
        var url = Path.Combine("https://pictures-api.migra.ml/", $"wwwroot/pictures/{picId}.webp");
        
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://nsfw-images-detection-and-classification.p.rapidapi.com/adult-content"),
            Headers =
            {
                { "x-rapidapi-host", "nsfw-images-detection-and-classification.p.rapidapi.com" },
                { "x-rapidapi-key", "faf9137d03msh62cdf2d3a847bc1p197d35jsn4f22cd1ac52c" },
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