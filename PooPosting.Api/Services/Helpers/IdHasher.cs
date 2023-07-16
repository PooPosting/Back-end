using HashidsNet;
using PooPosting.Api.Models.Configuration;

namespace PooPosting.Api.Services.Helpers;

public static class IdHasher
{
    private static int _minHashLength;
    private static string _hashAlphabet;
    private static Hashids _pictureHasher;
    private static Hashids _accountHasher;
    private static Hashids _commentHasher;

    public static void Configure(IConfiguration configuration)
    {
        var hashIdsSettings = new HashIdsSettings();
        configuration.GetSection("HashIdsSettings").Bind(hashIdsSettings);

        _minHashLength = hashIdsSettings.MinHashLength;
        _hashAlphabet = hashIdsSettings.HashAlphabet;
        _pictureHasher = new Hashids(hashIdsSettings.PictureSalt, _minHashLength, _hashAlphabet);
        _accountHasher = new Hashids(hashIdsSettings.AccountSalt, _minHashLength, _hashAlphabet);
        _commentHasher = new Hashids(hashIdsSettings.CommentSalt, _minHashLength, _hashAlphabet);
    }

    public static string EncodePictureId(int id)
    {
        return _pictureHasher.Encode(id);
    }

    public static int DecodePictureId(string hashedId)
    {
        return _pictureHasher.DecodeSingle(hashedId);
    }

    public static string EncodeAccountId(int id)
    {
        return _accountHasher.Encode(id);
    }

    public static int DecodeAccountId(string hashedId)
    {
        return _accountHasher.DecodeSingle(hashedId);
    }

    public static string EncodeCommentId(int id)
    {
        return _commentHasher.Encode(id);
    }

    public static int DecodeCommentId(string hashedId)
    {
        return _commentHasher.DecodeSingle(hashedId);
    }
}