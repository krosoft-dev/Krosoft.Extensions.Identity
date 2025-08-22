using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Services;

public class KrosoftTokenBuilderService : IKrosoftTokenBuilderService
{
    private readonly IIdentityService _identityService;

    public KrosoftTokenBuilderService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public KrosoftToken Build()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = _identityService.GetId(),
            Name = _identityService.GetName(),
            Email = _identityService.GetEmail(),
            RoleId = _identityService.GetRoleId(),
            LangueId = _identityService.GetLangueId(),
            LangueCode = _identityService.GetLangueCode()
        };

        var tenantsId = _identityService.GetTenantsId().ToHashSet();
        foreach (var tenantId in tenantsId)
        {
            krosoftToken.TenantsId.Add(tenantId);
        }

        var permissions = _identityService.GetPermissions().ToHashSet();
        foreach (var permission in permissions)
        {
            krosoftToken.Permissions.Add(permission);
        }

        return krosoftToken;
    }
}