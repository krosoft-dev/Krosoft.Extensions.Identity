using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
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

    [TestMethod]
    public void Transform_WithJsonArrayClaim_ShouldTransformToMultipleClaims()
    {
        var jsonArray = "[\"value1\", \"value2\", \"value3\"]";
        var claims = new List<Claim>
        {
            new Claim("testType", jsonArray, JsonClaimValueTypes.JsonArray),
            new Claim("normalType", "normalValue", ClaimValueTypes.String)
        };

        var result = claims.Transform().ToList();

        Check.That(result).HasSize(4);
        Check.That(result.Where(c => c.Type == "testType")).HasSize(3);
        Check.That(result.Select(x => x.Type)).ContainsExactly("testType", "testType", "testType", "normalType");
        Check.That(result.Select(x => x.Value)).ContainsExactly("value1", "value2", "value3", "normalValue");
    }

    [TestMethod]
    public void Transform_WithNormalClaims_ShouldReturnSameClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("type1", "value1", ClaimValueTypes.String),
            new Claim("type2", "value2", ClaimValueTypes.String)
        };

        var result = claims.Transform().ToList();

        Check.That(result).HasSize(2);
        Check.That(result.Select(x => x.Type)).ContainsExactly("type1", "type2");
        Check.That(result.Select(x => x.Value)).ContainsExactly("value1", "value2");
    }

    [TestMethod]
    public void Transform_WithEmptyClaims_ShouldReturnEmptyList()
    {
        var result = new List<Claim>().Transform().ToList();

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void TransformJsonArray_WithJsonArrayValueType_ShouldReturnMultipleClaims()
    {
        var jsonArray = "[\"value1\", \"value2\", \"value3\"]";
        var claim = new Claim("testType", jsonArray, JsonClaimValueTypes.JsonArray);

        var result = claim.TransformJsonArray().ToList();

        Check.That(result).HasSize(3);
        Check.That(result.Select(x => x.Type)).ContainsExactly("testType", "testType", "testType");
        Check.That(result.Select(x => x.Value)).ContainsExactly("value1", "value2", "value3");
        Check.That(result.All(c => c.ValueType == JsonClaimValueTypes.JsonArray)).IsTrue();
    }

    [TestMethod]
    public void TransformJsonArray_WithNormalValueType_ShouldReturnSameClaim()
    {
        var claim = new Claim("testType", "testValue", ClaimValueTypes.String);

        var result = claim.TransformJsonArray().ToList();

        Check.That(result).HasSize(1);
        Check.That(result.First()).IsEqualTo(claim);
    }

    [TestMethod]
    public void TransformJsonArray_WithEmptyJsonArray_ShouldReturnEmptyList()
    {
        var claim = new Claim("testType", "[]", JsonClaimValueTypes.JsonArray);

        var result = claim.TransformJsonArray().ToList();

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void TransformJsonArray_WithNullJsonArray_ShouldReturnEmptyList()
    {
        var claim = new Claim("testType", "null", JsonClaimValueTypes.JsonArray);

        var result = claim.TransformJsonArray().ToList();

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void TransformJsonArray_WithInvalidJson_ShouldThrowJsonException()
    {
        var claim = new Claim("testType", "invalid json", JsonClaimValueTypes.JsonArray);

        var action = () => claim.TransformJsonArray().ToList();
        Check.ThatCode(action).Throws<JsonException>();
    }

    [TestMethod]
    public void SetClaimValue_WithStringValue_ShouldAddStringClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = "testValue";

        claims.SetClaimValue(type, value, ClaimValueTypes.String);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().Value).IsEqualTo(value);
        Check.That(claims.First().ValueType).IsEqualTo(ClaimValueTypes.String);
    }

    [TestMethod]
    public void SetClaimValue_WithNullStringValue_ShouldNotAddClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        string? value = null;

        claims.SetClaimValue(type, value, ClaimValueTypes.String);

        Check.That(claims).IsEmpty();
    }

    [TestMethod]
    public void SetClaimValue_WithEmptyStringValue_ShouldNotAddClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = "";

        claims.SetClaimValue(type, value, ClaimValueTypes.String);

        Check.That(claims).IsEmpty();
    }

    [TestMethod]
    public void SetClaimValue_WithWhitespaceStringValue_ShouldNotAddClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = "   ";

        claims.SetClaimValue(type, value, ClaimValueTypes.String);

        Check.That(claims).IsEmpty();
    }

    [TestMethod]
    public void SetClaimValue_WithGenericString_ShouldAddStringClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = "testValue";

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().Value).IsEqualTo(value);
        Check.That(claims.First().ValueType).IsEqualTo(ClaimValueTypes.String);
    }

    [TestMethod]
    public void SetClaimValue_WithGenericBool_ShouldAddBooleanClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = true;

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().Value).IsEqualTo("True");
        Check.That(claims.First().ValueType).IsEqualTo(ClaimValueTypes.Boolean);
    }

    [TestMethod]
    public void SetClaimValue_WithGenericInt_ShouldAddIntegerClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = 42;

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().Value).IsEqualTo("42");
        Check.That(claims.First().ValueType).IsEqualTo(ClaimValueTypes.Integer);
    }

    [TestMethod]
    public void SetClaimValue_WithGenericStringArray_ShouldAddJsonArrayClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = new[] { "value1", "value2", "value3" };

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().ValueType).IsEqualTo(JsonClaimValueTypes.JsonArray);

        var deserializedValue = JsonSerializer.Deserialize<string[]>(claims.First().Value);
        Check.That(deserializedValue).IsEqualTo(value);
    }

    [TestMethod]
    public void SetClaimValue_WithGenericEmptyArray_ShouldNotAddClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = new string[0];

        claims.SetClaimValue(type, value);

        Check.That(claims).IsEmpty();
    }

    [TestMethod]
    public void SetClaimValue_WithGenericNullValue_ShouldNotAddClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        string? value = null;

        claims.SetClaimValue(type, value);

        Check.That(claims).IsEmpty();
    }

    [TestMethod]
    public void SetClaimValue_WithObjectAsString_ShouldAddStringClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        object value = "testValue";

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().Value).IsEqualTo("testValue");
        Check.That(claims.First().ValueType).IsEqualTo(ClaimValueTypes.String);
    }

    [TestMethod]
    public void SetClaimValue_WithObjectAsBool_ShouldAddBooleanClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        object value = true;

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().Value).IsEqualTo("True");
        Check.That(claims.First().ValueType).IsEqualTo(ClaimValueTypes.Boolean);
    }

    [TestMethod]
    public void SetClaimValue_WithObjectAsInt_ShouldAddIntegerClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        object value = 42;

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().Value).IsEqualTo("42");
        Check.That(claims.First().ValueType).IsEqualTo(ClaimValueTypes.Integer);
    }

    [TestMethod]
    public void SetClaimValue_WithObjectAsArray_ShouldAddJsonArrayClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        object value = new[] { "value1", "value2", "value3" };

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().ValueType).IsEqualTo(JsonClaimValueTypes.JsonArray);

        var deserializedValue = JsonSerializer.Deserialize<string[]>(claims.First().Value);
        Check.That(deserializedValue).IsEqualTo(new[] { "value1", "value2", "value3" });
    }

    [TestMethod]
    public void SetClaimValue_WithComplexObject_ShouldAddJsonClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = new { Name = "Test", Age = 30 };

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().ValueType).IsEqualTo(JsonClaimValueTypes.Json);

        var deserializedValue = JsonSerializer.Deserialize<dynamic>(claims.First().Value) as object;
        Check.That(deserializedValue).IsNotNull();
    }

    [TestMethod]
    public void SetClaimValue_WithMultipleCalls_ShouldAddMultipleClaims()
    {
        var claims = new List<Claim>();

        claims.SetClaimValue("type1", "value1");
        claims.SetClaimValue("type2", 42);
        claims.SetClaimValue("type3", true);

        Check.That(claims).HasSize(3);
        Check.That(claims.Select(x => x.Type)).ContainsExactly("type1", "type2", "type3");
        Check.That(claims.Select(x => x.Value)).ContainsExactly("value1", "42", "True");
    }

    [TestMethod]
    public void SetClaimValue_WithSameTypeMultipleTimes_ShouldAddMultipleClaims()
    {
        var claims = new List<Claim>();
        var type = "testType";

        claims.SetClaimValue(type, "value1");
        claims.SetClaimValue(type, "value2");

        Check.That(claims).HasSize(2);
        Check.That(claims.Select(x => x.Type)).ContainsExactly("testType", "testType");
        Check.That(claims.Select(x => x.Value)).ContainsExactly("value1", "value2");
    }

    [TestMethod]
    public void SetClaimValue_WithList_ShouldAddJsonArrayClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = new List<string> { "value1", "value2", "value3" };

        claims.SetClaimValue(type, value);

        Check.That(claims).HasSize(1);
        Check.That(claims.First().Type).IsEqualTo(type);
        Check.That(claims.First().ValueType).IsEqualTo(JsonClaimValueTypes.JsonArray);

        var deserializedValue = JsonSerializer.Deserialize<string[]>(claims.First().Value);
        Check.That(deserializedValue).IsEqualTo(value);
    }

    [TestMethod]
    public void SetClaimValue_WithEmptyList_ShouldNotAddClaim()
    {
        var claims = new List<Claim>();
        var type = "testType";
        var value = new List<string>();

        claims.SetClaimValue(type, value);

        Check.That(claims).IsEmpty();
    }
}