using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IClaimsService _claimsService;

    public IdentityService(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }

    public string? GetId() => _claimsService.CheckClaim(KrosoftClaimNames.Id);

    public T? GetId<T>() => ToType<T>(GetId());
    public string? Get(string claimName) => _claimsService.CheckClaim(claimName);
    public string? GetName() => _claimsService.CheckClaim(KrosoftClaimNames.Name);
    public string? GetEmail() => _claimsService.CheckClaim(KrosoftClaimNames.Email);
    public string? GetRoleId() => _claimsService.CheckClaim(KrosoftClaimNames.RoleId);
    public T? GetRoleId<T>() => ToType<T>(GetRoleId());

    public string? GetLangueId() => _claimsService.CheckClaim(KrosoftClaimNames.LangueId);
    public T? GetLangueId<T>() => ToType<T>(GetLangueId());

    public string? GetLangueCode() => _claimsService.CheckClaim(KrosoftClaimNames.LangueCode);

    public IEnumerable<T> GetTenantsId<T>() => GetTenantsId().Select(ToType<T>).ToList()!;

    public bool HasTenantsId(string tenantId) => GetTenantsId().Contains(tenantId);

    public IEnumerable<string> GetPermissions()
        => _claimsService.CheckClaims(KrosoftClaimNames.Permissions, claim => claim, false) ?? [];

    public IEnumerable<string> GetTenantsId()
        => _claimsService.CheckClaims(KrosoftClaimNames.TenantsId, claim => claim, false) ?? [];

    public T GetUniqueTenantId<T>()
    {
        var tenantsId = GetTenantsId().ToList();
        if (!tenantsId.Any())
        {
            throw new KrosoftTechnicalException("Aucun TenantId trouvé.");
        }

        if (tenantsId.Count > 1)
        {
            throw new KrosoftTechnicalException("Plusieurs TenantId trouvés.");
        }

        var tenantId = tenantsId.Single();

        return ToType<T>(tenantId)!;
    }

    public bool HasPermissions(string permission) => GetPermissions().Contains(permission);

    private static T? ToType<T>(string? tenantId)
    {
        if (typeof(T) == typeof(Guid))
        {
            if (Guid.TryParse(tenantId, out var guidResult))
            {
                return (T?)(object?)guidResult;
            }
        }

        if (typeof(T) == typeof(string))
        {
            return (T?)(object?)tenantId;
        }

        if (typeof(T) == typeof(int))
        {
            if (int.TryParse(tenantId, out var intResult))
            {
                return (T?)(object?)intResult;
            }
        }

        if (typeof(T) == typeof(long))
        {
            if (long.TryParse(tenantId, out var longResult))
            {
                return (T?)(object?)longResult;
            }
        }

        throw new KrosoftTechnicalException($"Impossible de convertir le TenantId '{tenantId}' en {typeof(T)}.");
    }
}