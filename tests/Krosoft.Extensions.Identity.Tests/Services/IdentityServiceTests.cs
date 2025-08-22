using System.Security.Claims;
using JetBrains.Annotations;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Services;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
[TestSubject(typeof(IdentityService))]
public class IdentityServiceTests : BaseTest
{
    private const string ClaimValueName = "Luke";

    //TestInitialize
    private IIdentityService _identityService = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddWebApiIdentityEx();

        MockClaims(services);
    }

    [TestMethod]
    public void GetName()
    {
        Check.That(_identityService.GetName()).IsEqualTo(ClaimValueName);
    }

    private static void MockClaims(IServiceCollection services)
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(KrosoftClaimNames.Id, Guid.CreateVersion7().ToString()),
            new Claim(KrosoftClaimNames.Name, ClaimValueName)
            //new Claim(KrosoftClaimNames.RoleIsInterne, true.ToString())
        }));

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext { User = claimsPrincipal };

        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

        services.AddSingleton(mockHttpContextAccessor.Object);
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _identityService = serviceProvider.GetRequiredService<IIdentityService>();
    }
}