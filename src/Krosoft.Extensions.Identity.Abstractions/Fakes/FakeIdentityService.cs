using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Abstractions.Fakes;

public class FakeIdentityService : IIdentityService
{
    public string GetId() => throw new NotImplementedException();
    public T GetId<T>() => throw new NotImplementedException();
    public string Get(string claimName) => throw new NotImplementedException();

    public string? GetName() => throw new NotImplementedException();
    public string? GetEmail() => throw new NotImplementedException();

    string IIdentityService.GetRoleId() => throw new NotImplementedException();

    string IIdentityService.GetLangueId() => throw new NotImplementedException();

    public string GetLangueCode() => throw new NotImplementedException();

    public IEnumerable<string> GetTenantsId() => throw new NotImplementedException();

    public IEnumerable<T> GetTenantsId<T>() => throw new NotImplementedException();

    public T GetUniqueTenantId<T>() => throw new NotImplementedException();

    public bool HasTenantsId(string tenantId) => throw new NotImplementedException();

    public IEnumerable<string> GetPermissions() => throw new NotImplementedException();

    public bool HasPermissions(string permission) => throw new NotImplementedException();

    public Guid GetRoleId() => throw new NotImplementedException();

    public Guid GetLangueId() => throw new NotImplementedException();
}