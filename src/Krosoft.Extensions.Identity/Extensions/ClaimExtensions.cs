using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Krosoft.Extensions.Identity.Extensions;

public static class ClaimExtensions
{
    private static readonly JsonSerializerOptions Settings = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static IEnumerable<Claim> Transform(this IEnumerable<Claim> claims)
    {
        var transformedClaims = new List<Claim>();
        foreach (var claim in claims)
        {
            var claimsss = claim.TransformJsonArray();
            transformedClaims.AddRange(claimsss);
        }

        return transformedClaims;
    }

    public static IEnumerable<Claim> TransformJsonArray(this Claim payload)
    {
        if (payload.ValueType == JsonClaimValueTypes.JsonArray)
        {
            var values = JsonSerializer.Deserialize<string[]>(payload.Value, Settings);
            return values?.Select(value => new Claim(payload.Type, value, payload.ValueType)).ToList() ?? new List<Claim>();
        }

        return new List<Claim> { payload };
    }

    public static void SetClaimValue(this ICollection<Claim> claims, string type, string? stringValue, string claimValueType)
    {
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return;
        }

        claims.Add(new Claim(type, stringValue, claimValueType));
    }

    public static void SetClaimValue<T>(this ICollection<Claim> claims, string type, T? value)
    {
        if (value is null)
        {
            return;
        }

        if (typeof(T) == typeof(string))
        {
            claims.SetClaimValue(type, value.ToString(), ClaimValueTypes.String);
            return;
        }

        if (typeof(T) == typeof(bool))
        {
            claims.SetClaimValue(type, value.ToString(), ClaimValueTypes.Boolean);
            return;
        }

        if (typeof(T) == typeof(int))
        {
            claims.SetClaimValue(type, value.ToString(), ClaimValueTypes.Integer);
            return;
        }

        if (value is IEnumerable<object> collection)
        {
            if (!collection.Any())
            {
                return;
            }

            claims.SetClaimValue(type, JsonSerializer.Serialize(value, Settings), JsonClaimValueTypes.JsonArray);
            return;
        }

        if (typeof(T) == typeof(object))
        {
            switch (value)
            {
                case string stringValue:
                    claims.SetClaimValue(type, stringValue, ClaimValueTypes.String);
                    return;
                case bool boolValue:
                    claims.SetClaimValue(type, boolValue.ToString(), ClaimValueTypes.Boolean);
                    return;
                case int intValue:
                    claims.SetClaimValue(type, intValue.ToString(), ClaimValueTypes.Integer);
                    return;
                case IEnumerable<object> collection1 when collection1.Any():
                    claims.SetClaimValue(type, JsonSerializer.Serialize(value, Settings), JsonClaimValueTypes.JsonArray);
                    return;
            }
        }

        claims.SetClaimValue(type, JsonSerializer.Serialize(value, Settings), JsonClaimValueTypes.Json);
    }
}