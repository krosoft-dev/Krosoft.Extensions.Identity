using JetBrains.Annotations;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Identity.Services;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
[TestSubject(typeof(ClaimsBuilderService))]
public class ClaimsBuilderServiceTests : BaseTest
{
    //TestInitialize
    private IClaimsBuilderService _claimsBuilderService = null!;

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _claimsBuilderService = serviceProvider.GetRequiredService<IClaimsBuilderService>();
    }

    [TestMethod]
    public void Build_Id()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(1);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id");
    }

    [TestMethod]
    public void Build_Name()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Name = "Claim_Name"
        };

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(2);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "name");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "Claim_Name");
    }

    [TestMethod]
    public void Build_Email()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Name = "Claim_Name",
            Email = "Claim_Email"
        };

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(3);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "name", "email");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "Claim_Name", "Claim_Email");
    }

    [TestMethod]
    public void Build_RoleId()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Name = "Claim_Name",
            Email = "Claim_Email",
            RoleId = "Claim_RoleId"
        };

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(4);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "name", "email", "roleId");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "Claim_Name", "Claim_Email", "Claim_RoleId");
    }

    [TestMethod]
    public void Build_Properties()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id",
            Name = "Claim_Name",
            Email = "Claim_Email",
            RoleId = "Claim_RoleId",
            LangueId = "Claim_LangueId",
            LangueCode = "Claim_LangueCode"
        };
        krosoftToken.AddProperty("tenantId", new Guid("00000000-1111-1111-1111-000000000001").ToString());
        krosoftToken.AddProperty("roleHomePage", "Claim_RoleHomePage");
        krosoftToken.AddProperty("roleIsInterne", true);

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims).HasSize(9);
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "name", "email", "roleId", "langueId", "langueCode", "tenantId", "roleHomePage", "roleIsInterne");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "Claim_Name", "Claim_Email", "Claim_RoleId", "Claim_LangueId", "Claim_LangueCode", "00000000-1111-1111-1111-000000000001", "Claim_RoleHomePage", "True");
    }

    [TestMethod]
    public void Build_Properties_StringValue()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("customString", "stringValue");

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "customString");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "stringValue");
    }

    [TestMethod]
    public void Build_Properties_BooleanValue()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("RoleIsInterne", true);

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "RoleIsInterne");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "True");
    }

    [TestMethod]
    public void Build_Properties_IntegerValue()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("customInt", 42);

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "customInt");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "42");
    }

    [TestMethod]
    public void Build_Properties_ArrayValue()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("customArray", new[] { "value1", "value2" });

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "customArray");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "[\"value1\",\"value2\"]");
    }

    [TestMethod]
    public void Build_Properties_EmptyArrayValue()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("customArray", Array.Empty<string>());

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id");
    }

    [TestMethod]
    public void Build_Properties_ComplexObject()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = "Claim_Id"
        };
        krosoftToken.AddProperty("customObject", new { Name = "test", Value = 42 });

        var claims = _claimsBuilderService.Build(krosoftToken).ToList();

        Check.That(claims).IsNotNull();
        Check.That(claims.Select(c => c.Type)).ContainsExactly("id", "customObject");
        Check.That(claims.Select(c => c.Value)).ContainsExactly("Claim_Id", "{\"Name\":\"test\",\"Value\":42}");
    }

    [TestMethod]
    public void Build_Null()
    {
        Check.ThatCode(() => { _claimsBuilderService.Build(null); })
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'krosoftToken' n'est pas renseignée.");
    }

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityEx();
        services.AddTransient<IClaimsBuilderService, ClaimsBuilderService>();

        base.AddServices(services, configuration);
    }
}