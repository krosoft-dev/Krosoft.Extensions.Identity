namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IIdentityService
{
    string? GetId();
    T? GetId<T>();
    string? Get(string claimName);
    string? GetName();
    string? GetEmail();
    string? GetRoleId();
    string? GetLangueId();
    string? GetLangueCode();
    IEnumerable<string> GetTenantsId();
    IEnumerable<T> GetTenantsId<T>();
    T GetUniqueTenantId<T>();
    bool HasTenantsId(string tenantId);
    IEnumerable<string> GetPermissions();
    bool HasPermissions(string permission);
}