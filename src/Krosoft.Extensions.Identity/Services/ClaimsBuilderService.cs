using System.Security.Claims;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;

namespace Krosoft.Extensions.Identity.Services;

public class ClaimsBuilderService : IClaimsBuilderService
{
    public IEnumerable<Claim> Build(KrosoftToken? krosoftToken)
    {
        Guard.IsNotNull(nameof(krosoftToken), krosoftToken);
        Guard.IsNotNullOrWhiteSpace(nameof(krosoftToken.Id), krosoftToken!.Id);

        var claims = new List<Claim>();

        claims.SetClaimValue(KrosoftClaimNames.Id, krosoftToken.Id);
        claims.SetClaimValue(KrosoftClaimNames.Name, krosoftToken.Name);
        claims.SetClaimValue(KrosoftClaimNames.Email, krosoftToken.Email, ClaimValueTypes.Email);
        claims.SetClaimValue(KrosoftClaimNames.RoleId, krosoftToken.RoleId);
        claims.SetClaimValue(KrosoftClaimNames.LangueId, krosoftToken.LangueId);
        claims.SetClaimValue(KrosoftClaimNames.LangueCode, krosoftToken.LangueCode);
        claims.SetClaimValue(KrosoftClaimNames.Permissions, krosoftToken.Permissions);
        claims.SetClaimValue(KrosoftClaimNames.TenantsId, krosoftToken.TenantsId);

        foreach (var property in krosoftToken.Properties)
        {
            claims.SetClaimValue(property.Key, property.Value);
        }

        return claims;
    }
}