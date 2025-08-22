using JetBrains.Annotations;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Identity.Services;
using Krosoft.Extensions.Identity.Tests.Core;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
[TestSubject(typeof(IdentityService))]
public class IdentityServiceTests : BaseTest
{
    private const string ClaimValueName = "Luke";
    private const string ClaimValueEmail = "Luke@Skywalker.sw";
    private const string ClaimValueRoleId = "some-id";
    private const string ClaimValueLangueId = "fr";
    private const string ClaimValueLangueCode = "FR";
    private static readonly IEnumerable<string> ClaimValueTenantsId = ["a", "b"];
    private static readonly IEnumerable<string> ClaimValuePermissions = ["read", "write"];
    private static readonly Guid ClaimValueId = new Guid("f247e235-318f-4b0c-8220-6a23669db3f5");
    private static readonly Guid ClaimValueTenantId1 = new Guid("0bc52175-7774-4f0b-833e-ae89a12d24e4");

    //TestInitialize
    private IIdentityService _identityService = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityEx()
                .AddTransient<IClaimsService, InMemoryClaimsService>();

        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.Id, [ClaimValueId.ToString()] },
            { KrosoftClaimNames.Name, [ClaimValueName] },
            { KrosoftClaimNames.Email, [ClaimValueEmail] },
            { KrosoftClaimNames.RoleId, [ClaimValueRoleId] },
            { KrosoftClaimNames.LangueId, [ClaimValueLangueId] },
            { KrosoftClaimNames.LangueCode, [ClaimValueLangueCode] },
            { KrosoftClaimNames.TenantsId, ClaimValueTenantsId.ToList() },
            { KrosoftClaimNames.Permissions, ClaimValuePermissions.ToList() }
        };
        services.AddSingleton(claims);
    }

    [TestMethod]
    public void Get_Ok()
    {
        Check.That(_identityService.Get(KrosoftClaimNames.Name)).IsEqualTo(ClaimValueName);
    }

    [TestMethod]
    public void Get_Ko()
    {
        Check.ThatCode(() => _identityService.Get("KrosoftClaimNames"))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Le claim 'KrosoftClaimNames' n'existe pas.");
    }

    [TestMethod]
    public void GetId()
    {
        Check.That(_identityService.GetId()).IsEqualTo(ClaimValueId.ToString());
    }

    [TestMethod]
    public void GetId_Generic()
    {
        Check.That(_identityService.GetId<Guid>()).IsEqualTo(ClaimValueId);
    }

    [TestMethod]
    public void GetLangueId_Generic_Ko()
    {
        Check.ThatCode(() => _identityService.GetLangueId<long>())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible de convertir le TenantId 'fr' en System.Int64.");
    }

    [TestMethod]
    public void GetLangueId_Generic()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.LangueId, [42.ToString()] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetLangueId<long>()).IsEqualTo(42);
    }

    [TestMethod]
    public void GetRoleId_Generic_Ko()
    {
        Check.ThatCode(() => _identityService.GetRoleId<Guid>())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible de convertir le TenantId 'some-id' en System.Guid.");
    }

    [TestMethod]
    public void GetRoleId_Generic_Ko_Int()
    {
        Check.ThatCode(() => _identityService.GetRoleId<int>())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible de convertir le TenantId 'some-id' en System.Int32.");
    }

    [TestMethod]
    public void GetRoleId_Generic_Ko_Long()
    {
        Check.ThatCode(() => _identityService.GetRoleId<long>())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible de convertir le TenantId 'some-id' en System.Int64.");
    }

    [TestMethod]
    public void GetRoleId_Generic()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.RoleId, [24.ToString()] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetRoleId<int>()).IsEqualTo(24);
    }

    [TestMethod]
    public void GetRoleId_Generic_String()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.RoleId, [24.ToString()] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetRoleId<string>()).IsEqualTo("24");
    }

    [TestMethod]
    public void GetName()
    {
        Check.That(_identityService.GetName()).IsEqualTo(ClaimValueName);
    }

    [TestMethod]
    public void GetEmail()
    {
        Check.That(_identityService.GetEmail()).IsEqualTo(ClaimValueEmail);
    }

    [TestMethod]
    public void GetRoleId()
    {
        Check.That(_identityService.GetRoleId()).IsEqualTo(ClaimValueRoleId);
    }

    [TestMethod]
    public void GetLangueId()
    {
        Check.That(_identityService.GetLangueId()).IsEqualTo(ClaimValueLangueId);
    }

    [TestMethod]
    public void GetLangueCode()
    {
        Check.That(_identityService.GetLangueCode()).IsEqualTo(ClaimValueLangueCode);
    }

    [TestMethod]
    public void GetTenantsId()
    {
        Check.That(_identityService.GetTenantsId()).ContainsExactly(ClaimValueTenantsId);
    }

    [TestMethod]
    public void GetTenantsId_Generic()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.TenantsId, [ClaimValueTenantId1.ToString()] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetTenantsId<Guid>()).ContainsExactly(ClaimValueTenantId1);
    }

    [TestMethod]
    public void GetPermissions_EmptyNoClaim()
    {
        var claims = new Dictionary<string, List<string>>();
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetPermissions()).IsEmpty();
    }

    [TestMethod]
    public void GetPermissions_Empty()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.Permissions, [] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetPermissions()).IsEmpty();
    }

    [TestMethod]
    public void GetTenantsId_Generic_Ko()
    {
        Check.ThatCode(() => _identityService.GetTenantsId<Guid>())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible de convertir le TenantId 'a' en System.Guid.");
    }

    [TestMethod]
    public void GetUniqueTenantId_Generic_Ko_Multiple()
    {
        Check.ThatCode(() => _identityService.GetUniqueTenantId<Guid>())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Plusieurs TenantId trouvés.");
    }

    [TestMethod]
    public void GetUniqueTenantId_Generic_Ko_()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.TenantsId, [] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();

        Check.ThatCode(() => _identityService.GetUniqueTenantId<Guid>())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Aucun TenantId trouvé.");
    }

    [TestMethod]
    public void GetUniqueTenantId_Generic()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.TenantsId, [ClaimValueTenantId1.ToString()] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetUniqueTenantId<Guid>()).IsEqualTo(ClaimValueTenantId1);
    }

    [TestMethod]
    public void GetUniqueTenantId_Ko_Multiple()
    {
        Check.ThatCode(() => _identityService.GetUniqueTenantId())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Plusieurs TenantId trouvés.");
    }

    [TestMethod]
    public void GetUniqueTenantId_Ko_()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.TenantsId, [] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();

        Check.ThatCode(() => _identityService.GetUniqueTenantId())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Aucun TenantId trouvé.");
    }

    [TestMethod]
    public void GetUniqueTenantId()
    {
        var claims = new Dictionary<string, List<string>>
        {
            { KrosoftClaimNames.TenantsId, [ClaimValueTenantId1.ToString()] }
        };
        using var serviceProvider = CreateServiceCollection(services => { services.AddSingleton(claims); });
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
        Check.That(_identityService.GetUniqueTenantId()).IsEqualTo(ClaimValueTenantId1.ToString());
    }

    [TestMethod]
    public void HasTenantsId_True()
    {
        Check.That(_identityService.HasTenantsId("a")).IsTrue();
    }

    [TestMethod]
    public void HasTenantsId_False()
    {
        Check.That(_identityService.HasTenantsId("someTenantId")).IsFalse();
    }

    [TestMethod]
    public void GetPermissions()
    {
        Check.That(_identityService.GetPermissions()).ContainsExactly(ClaimValuePermissions);
    }

    [TestMethod]
    public void HasPermissions_True()
    {
        Check.That(_identityService.HasPermissions("read")).IsTrue();
    }

    [TestMethod]
    public void HasPermissions_False()
    {
        Check.That(_identityService.HasPermissions("somePermission")).IsFalse();
    }

    //private static void MockClaims(IServiceCollection services)
    //{
    //    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    //    {
    //        new Claim(KrosoftClaimNames.Id, Guid.CreateVersion7().ToString()),
    //        new Claim(KrosoftClaimNames.Name, ClaimValueName)
    //        //new Claim(KrosoftClaimNames.RoleIsInterne, true.ToString())
    //    }));

    //    var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
    //    var context = new DefaultHttpContext { User = claimsPrincipal };

    //    mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

    //    services.AddSingleton(mockHttpContextAccessor.Object);
    //}

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
    }
}