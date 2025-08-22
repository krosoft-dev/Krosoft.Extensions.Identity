using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Identity.Services;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
[TestSubject(typeof(KrosoftTokenBuilderService))]
public class KrosoftTokenBuilderServiceTests : BaseTest
{
    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityEx();
    }

    [TestMethod]
    public void Build_Empty()
    {
        void GetServices(IServiceCollection services)
        {
            var mockUserProvider = new Mock<IIdentityService>();
            services.SwapTransient(_ => mockUserProvider.Object);
        }

        using var serviceProvider = CreateServiceCollection(GetServices);
        var krosoftTokenBuilderService = serviceProvider.GetRequiredService<IKrosoftTokenBuilderService>();

        var result = krosoftTokenBuilderService.Build();

        Check.That(result).IsNotNull();
        Check.That(result.Id).IsEqualTo(null);
        //Check.That(result.TenantId).IsEqualTo(null);
    }

    [TestMethod]
    public void Build_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            var mockUserProvider = new Mock<IIdentityService>();
            mockUserProvider.Setup(userProvider => userProvider.GetId()).Returns(1.ToGuid().ToString());
            //mockUserProvider.Setup(userProvider => userProvider.GetTenantId()).Returns("test");
            services.SwapTransient(_ => mockUserProvider.Object);
        }

        using var serviceProvider = CreateServiceCollection(GetServices);
        var krosoftTokenBuilderService = serviceProvider.GetRequiredService<IKrosoftTokenBuilderService>();

        var result = krosoftTokenBuilderService.Build();

        Check.That(result).IsNotNull();
        Check.That(result.Id).IsEqualTo("00000000-0000-0000-0000-000000000001");
        //Check.That(result.TenantId).IsEqualTo("test"); 
    }
}