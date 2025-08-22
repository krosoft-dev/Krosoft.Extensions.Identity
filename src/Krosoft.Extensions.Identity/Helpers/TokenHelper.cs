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
}