using HashidsNet;

namespace PooPosting.Api.Services.Helpers;

public static class IdHasher
{
    private const int MinHashLength = 7;
    private const string HashAlphabet = "-_1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static readonly Hashids PictureHasher = new("p!c700re$", MinHashLength, HashAlphabet);
    private static readonly Hashids AccountHasher = new("a$$0un70Z", MinHashLength, HashAlphabet);
    private static readonly Hashids CommentHasher = new("C00m3n700", MinHashLength, HashAlphabet);


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