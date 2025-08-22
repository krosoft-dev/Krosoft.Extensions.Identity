using Krosoft.Extensions.Identity.Helpers;

namespace Krosoft.Extensions.Identity.Tests.Helpers;

[TestClass]
public class TokenHelperTests
{
    private readonly string _salt = "salt";
    private readonly string _secret = "secret";

    [TestMethod]
    public void CreateSignedToken_ShouldReturnExpectedFormat()
    {
        var payloadJson = "{\"key\":\"value\"}";

        var result = TokenHelper.CreateSignedToken(payloadJson, _secret, _salt);

        Check.That(result).Contains(".");
        var parts = result.Split('.');
        Check.That(parts.Length).IsEqualTo(2);
    }

    [TestMethod]
    public void CreateSignedToken_ShouldReturnDifferentTokensForDifferentPayloads()
    {
        var payloadJson1 = "{\"key\":\"value1\"}";
        var payloadJson2 = "{\"key\":\"value2\"}";

        var result1 = TokenHelper.CreateSignedToken(payloadJson1, _secret, _salt);
        var result2 = TokenHelper.CreateSignedToken(payloadJson2, _secret, _salt);

        Check.That(result1).IsNotEqualTo(result2);
    }

    [TestMethod]
    public void CreateSignedToken_ShouldReturnSameTokenForSameInput()
    {
        var payloadJson = "{\"key\":\"value\"}";

        var result1 = TokenHelper.CreateSignedToken(payloadJson, _secret, _salt);
        var result2 = TokenHelper.CreateSignedToken(payloadJson, _secret, _salt);

        Check.That(result1).IsEqualTo(result2);
    }
}