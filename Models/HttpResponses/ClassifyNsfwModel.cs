using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PicturesAPI.Models.HttpResponses;

public class ClassifyNsfwModel
{
    [JsonProperty("detections")]
    public List<DetectedObject> Detections { get; set; }

    [JsonProperty("nsfw_score")]
    public double NsfwScore { get; set; }
    
}