using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Extensions;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Identity.Tests.Services;

[TestClass]
public class RefreshTokenGeneratorTests : BaseTest
{
    //TestInitialize
    private IRefreshTokenGenerator _refreshTokenGenerator = null!;

    [TestMethod]
    public void CreateToken_ShouldReturnUniqueTokens()
    {
        var token1 = _refreshTokenGenerator.CreateToken();
        var token2 = _refreshTokenGenerator.CreateToken();

        Check.That(token1).IsNotNull();
        Check.That(token2).IsNotNull();
        Check.That(token1).IsNotEqualTo(token2);
    }

    [TestMethod]
    public void CreateToken_ShouldReturnBase64String()
    {
        var token = _refreshTokenGenerator.CreateToken();

        Check.That(token).IsNotNull();
        var tokenBytes = Convert.FromBase64String(token);
        Check.That(tokenBytes.Length).IsEqualTo(32);
    }

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefreshTokenGenerator();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _refreshTokenGenerator = serviceProvider.GetRequiredService<IRefreshTokenGenerator>();
    }
}