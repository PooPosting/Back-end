using HashidsNet;

namespace PicturesAPI.Configuration;

public static class IdHasher
{
    private static Hashids PictureHasher { get; set; } = new Hashids("p!c700re$", 12);
    private static Hashids AccountHasher { get; set; } = new Hashids("a$$0un70Z", 12);
    private static Hashids CommentHasher { get; set; } = new Hashids("C00m3n700", 12);


    public static string EncodePictureId(int id)
    {
        return PictureHasher.Encode(id);
    }

    public static int DecodePictureId(string hashedId)
    {
        return PictureHasher.DecodeSingle(hashedId);
    }

    public static string EncodeAccountId(int id)
    {
        return AccountHasher.Encode(id);
    }

    public static int DecodeAccountId(string hashedId)
    {
        return AccountHasher.DecodeSingle(hashedId);
    }

    public static string EncodeCommentId(int id)
    {
        return CommentHasher.Encode(id);
    }

    public static int DecodeCommentId(string hashedId)
    {
        return CommentHasher.DecodeSingle(hashedId);
    }
}