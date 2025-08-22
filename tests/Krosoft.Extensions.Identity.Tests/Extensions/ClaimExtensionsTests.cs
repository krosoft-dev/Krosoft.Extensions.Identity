using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Krosoft.Extensions.Identity.Extensions;

namespace Krosoft.Extensions.Identity.Tests.Extensions;

[TestClass]
public class ClaimExtensionsTests
{
    [TestMethod]
    public void Transform_ShouldReturnTransformedClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("type1", "[\"value1\",\"value2\"]", JsonClaimValueTypes.JsonArray),
            new Claim("type2", "singleValue", ClaimValueTypes.String)
        };

        var result = claims.Transform().ToList();

        Check.That(result).HasSize(3);

        Check.That(result[0].Type).IsEqualTo("type1");
        Check.That(result[0].Value).IsEqualTo("value1");
        Check.That(result[0].ValueType).IsEqualTo(JsonClaimValueTypes.JsonArray);

        Check.That(result[1].Type).IsEqualTo("type1");
        Check.That(result[1].Value).IsEqualTo("value2");
        Check.That(result[1].ValueType).IsEqualTo(JsonClaimValueTypes.JsonArray);

        Check.That(result[2].Type).IsEqualTo("type2");
        Check.That(result[2].Value).IsEqualTo("singleValue");
        Check.That(result[2].ValueType).IsEqualTo(ClaimValueTypes.String);
    }

    [TestMethod]
    public void TransformJsonArray_ShouldReturnSingleClaim_WhenNotJsonArray()
    {
        var claim = new Claim("type", "singleValue", ClaimValueTypes.String);

        var result = claim.TransformJsonArray().ToList();

        Check.That(result).HasSize(1);
        Check.That(result[0].Type).IsEqualTo("type");
        Check.That(result[0].Value).IsEqualTo("singleValue");
        Check.That(result[0].ValueType).IsEqualTo(ClaimValueTypes.String);
    }

    [TestMethod]
    public void SetClaimValue_ShouldAddClaim_WhenValueIsNotNull()
    {
        var claims = new List<Claim>();

        claims.SetClaimValue("type", "value", ClaimValueTypes.String);

        Check.That(claims).HasSize(1);
        Check.That(claims[0].Type).IsEqualTo("type");
        Check.That(claims[0].Value).IsEqualTo("value");
        Check.That(claims[0].ValueType).IsEqualTo(ClaimValueTypes.String);
    }

    [TestMethod]
    public void SetClaimValueT_ShouldAddClaim_WhenValueIsNotNull()
    {
        var claims = new List<Claim>();

        claims.SetClaimValue("type", true);

        Check.That(claims).HasSize(1);
        Check.That(claims[0].Type).IsEqualTo("type");
        Check.That(claims[0].Value).IsEqualTo("True");
        Check.That(claims[0].ValueType).IsEqualTo(ClaimValueTypes.Boolean);
    }
    [TestMethod]
    public void SetClaimValue_Int()
    {
        var claims = new List<Claim>();

        claims.SetClaimValue("type", 42);

        Check.That(claims).HasSize(1);
        Check.That(claims[0].Type).IsEqualTo("type");
        Check.That(claims[0].Value).IsEqualTo("42");
        Check.That(claims[0].ValueType).IsEqualTo(ClaimValueTypes.Integer);
    }

    [TestMethod]
    public void SetClaimValueT_ShouldNotAddClaim_WhenValueIsNull()
    {
        var claims = new List<Claim>();

        claims.SetClaimValue<string>("type", null);

        Check.That(claims).IsEmpty();
    }
}