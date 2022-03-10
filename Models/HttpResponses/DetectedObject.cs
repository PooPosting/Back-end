using System.Text.Json.Serialization;

namespace PicturesAPI.Models.HttpResponses;

public class DetectedObject
{
    [JsonPropertyName("confidence")]
    public string Confidence { get; set; }

    [JsonPropertyName("bounding_box")]
    public List<int> BoundingBox { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}