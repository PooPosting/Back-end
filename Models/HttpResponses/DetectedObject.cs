using System.Text.Json.Serialization;

namespace PicturesAPI.Models.HttpResponses;

public class DetectedObject
{
    [JsonPropertyName("box")]
    public List<int> Box { get; set; }
    
    [JsonPropertyName("score")]
    public float Score { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }
}