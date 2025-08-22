using System.Security.Cryptography;
using System.Text;
using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Identity.Helpers;

public static class TokenHelper
{
    public static string CreateSignedToken(string payloadJson, string secret, string salt)
    {
        var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
        var signature = CryptographyHelper.HashSha256(payloadBase64, secret, salt);

        return $"{payloadBase64}.{signature}";
    }

    public static string? GetUnsignedPayload(string? signedToken,
                                             string secret,
                                             string salt)
    {
        if (string.IsNullOrEmpty(signedToken))
        {
            return null;
        }

        var parts = signedToken.Split('.');
        if (parts.Length != 2)
        {
            return null;
        }

        var payloadBase64 = parts[0];
        var providedSignature = parts[1];

        var expectedSignature = CryptographyHelper.HashSha256(payloadBase64, secret, salt);
        if (!SafeEquals(providedSignature, expectedSignature))
        {
            return null;
        }

        var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(payloadBase64));

        return payloadJson;
    }

    private static bool SafeEquals(string base64A, string base64B)
    {
        var aBytes = Convert.FromBase64String(base64A);
        var bBytes = Convert.FromBase64String(base64B);

        if (aBytes.Length != bBytes.Length)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
    }
}