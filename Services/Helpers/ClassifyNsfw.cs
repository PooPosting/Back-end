using Newtonsoft.Json;
using PicturesAPI.Models.HttpResponses;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Services.Helpers;
using DeepAI;

public class ClassifyNsfw : IClassifyNsfw
{
    public bool IsSafeForWork(string picId) {
        var api = new DeepAI_API(apiKey: "4d8f9815-1ea6-4d13-9c4a-c44ceba00dad");
        var url = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/pictures/{picId}.webp");
        StandardApiResponse resp;

        using (var stream = File.OpenRead(url))
        {
            resp = api.callStandardApi("content-moderation", new
            {
                image = stream
            });
        }
        
        var pictureClassified = JsonConvert.DeserializeObject<ClassifyNsfwModel>(api.objectAsJsonString(resp.output));
        return pictureClassified.NsfwScore < 0.6;
    }
}