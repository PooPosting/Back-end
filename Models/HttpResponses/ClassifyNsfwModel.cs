using Newtonsoft.Json;

namespace PicturesAPI.Models.HttpResponses;

public class ClassifyNsfwModel
{
    [JsonProperty("objects")]
    public List<DetectedObject> Objects { get; set; }

    [JsonProperty("unsafe")]
    public bool Unsafe { get; set; }
    
}