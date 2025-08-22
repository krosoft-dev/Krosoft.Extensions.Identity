using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Tests.Core;

public class InMemoryClaimsService : IClaimsService
{
    private readonly Dictionary<string, List<string>> _claims;

    public InMemoryClaimsService(Dictionary<string, List<string>> claims)
    {
        _claims = claims;
    }

    public T? CheckClaim<T>(string claimName, Func<string, T?> funcSuccess, bool isRequired)
    {
        if (_claims.TryGetValue(claimName, out var claimValues) && claimValues.Count > 0)
        {
            return funcSuccess(claimValues.First());
        }

        if (!isRequired)
        {
            return default;
        }

        throw new KrosoftTechnicalException($"Le claim {claimName} n'existe pas.");
    }

    public string? CheckClaim(string claimName) => CheckClaim<string>(claimName, (x) =>x, true);

    public T? CheckClaims<T>(string claimName, Func<IEnumerable<string>, T?> funcSuccess, bool isRequired)
    {
        if (_claims.TryGetValue(claimName, out var claimValues) && claimValues.Count > 0)
        {
            return funcSuccess(claimValues);
        }

        if (!isRequired)
        {
            return default;
        }

        throw new KrosoftTechnicalException($"Le claim {claimName} n'existe pas.");
    }
}