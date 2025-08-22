namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IClaimsService
{
    T? CheckClaim<T>(string claimName, Func<string, T> funcSucess, bool isRequired);
    string? CheckClaim(string claimName);
    T? CheckClaims<T>(string claimName, Func<IEnumerable<string>, T> funcSucess, bool isRequired);
}