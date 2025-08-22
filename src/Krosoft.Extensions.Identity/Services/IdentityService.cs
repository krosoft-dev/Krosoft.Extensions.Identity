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

    public string? GetId() => _claimsService.CheckClaim<string>(KrosoftClaimNames.Id);

    public T? GetId<T>() => ToType<T>(GetId());
    public string GetName() => throw new NotImplementedException();

    public string GetEmail() => throw new NotImplementedException();

    public string GetRoleId() => throw new NotImplementedException();

    public string GetLangueId() => throw new NotImplementedException();

    public string GetLangueCode() => throw new NotImplementedException();

    public IEnumerable<string> GetTenantsId() => throw new NotImplementedException();

    public IEnumerable<T> GetTenantsId<T>() => throw new NotImplementedException();

    public T GetUniqueTenantId<T>() => throw new NotImplementedException();

    public bool HasTenantsId(string tenantId) => throw new NotImplementedException();

    public IEnumerable<string> GetPermissions() => throw new NotImplementedException();

    public bool HasPermissions(string permission) => throw new NotImplementedException();

    //public string? GetName() => _claimsService.CheckClaim<string>(KrosoftClaimNames.Name);

    //public string? GetRoleId() => throw new NotImplementedException();

    //string? IIdentityService.GetLangueId() => throw new NotImplementedException();

    //public string? GetLangueCode()
    //{
    //    return _claimsService.CheckClaim(KrosoftClaimNames.LangueCode, claim => claim, true);
    //}

    //public IEnumerable<string> GetTenantsId()
    //{
    //    return _claimsService.CheckClaims(KrosoftClaimNames.TenantsId, claim => claim, false) ?? [];
    //}

    //public IEnumerable<T> GetTenantsId<T>() => GetTenantsId().Select(ToType<T>).ToList()!;

    //public T GetUniqueTenantId<T>()
    //{
    //    var tenantsId = GetTenantsId().ToList();
    //    if (!tenantsId.Any())
    //    {
    //        throw new KrosoftTechnicalException("Aucun TenantId trouvé.");
    //    }

    //    if (tenantsId.Count > 1)
    //    {
    //        throw new KrosoftTechnicalException("Plusieurs TenantId trouvés.");
    //    }

    //    var tenantId = tenantsId.Single();

    //    return ToType<T>(tenantId);
    //}

    //public bool HasTenantsId(string tenantId) => GetTenantsId().Contains(tenantId);

    //public IEnumerable<string> GetPermissions() => throw new NotImplementedException();

    //public bool HasPermissions(string permission) => GetPermissions().Contains(permission);

    //public Guid GetLangueId() => _claimsService.CheckClaim(KrosoftClaimNames.LangueId, claim => new Guid(claim), true);

    //public Guid GetRoleId()
    //{
    //    return _claimsService.CheckClaim(KrosoftClaimNames.RoleId, claim => new Guid(claim), true);
    //}

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