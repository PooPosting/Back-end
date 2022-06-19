
using Google.Cloud.Vision.V1;

namespace PicturesAPI.Services.Helpers;

public static class NsfwClassifier
{
    public async static Task<SafeSearchAnnotation> ClassifyAsync(byte[] fileBytes)
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
            Path.Combine(Environment.CurrentDirectory, "authKey.json"));

        var client = ImageAnnotatorClient.Create();
        var image = Image.FromBytes(fileBytes);

        return await client.DetectSafeSearchAsync(image);
    }
}
