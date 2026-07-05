using DpopPortfolio.Functions.Shared.Configuration;

namespace DpopPortfolio.Functions.Tests.Shared.Configuration;

public sealed class DpopPortfolioOptionsValidatorTests
{
    private readonly DpopPortfolioOptionsValidator _validator = new();

    [Fact]
    public void Validate_AcceptsDefaultLocalDevelopmentOptions()
    {
        var result = _validator.Validate(null, new DpopPortfolioOptions());

        Assert.True(result.Succeeded, string.Join(Environment.NewLine, result.Failures ?? []));
    }

    [Fact]
    public void Validate_RejectsHttpIssuerOutsideLocalDevelopment()
    {
        var options = new DpopPortfolioOptions
        {
            Issuer = "http://portfolio.example.com"
        };

        var result = _validator.Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Failures ?? [],
            failure => failure.Contains("Issuer must use HTTPS", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_RejectsUnsupportedCacheProvider()
    {
        var options = new DpopPortfolioOptions
        {
            CacheProvider = "Filesystem"
        };

        var result = _validator.Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Failures ?? [],
            failure => failure.Contains("CacheProvider", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_RejectsUnsafeAccessTokenLifetime()
    {
        var options = new DpopPortfolioOptions
        {
            TokenLifetimes = new TokenLifetimeOptions
            {
                AccessTokenMinutes = 120
            }
        };

        var result = _validator.Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains(
            result.Failures ?? [],
            failure => failure.Contains("AccessTokenMinutes", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ApplyEnvironmentFallbacks_UsesLegacyFlatEnvironmentVariables()
    {
        const string key = "DPOP_AUDIENCE";
        var previousValue = Environment.GetEnvironmentVariable(key);

        try
        {
            Environment.SetEnvironmentVariable(key, "legacy-audience");

            var options = new DpopPortfolioOptions();
            options.ApplyEnvironmentFallbacks();

            Assert.Equal("legacy-audience", options.Audience);
        }
        finally
        {
            Environment.SetEnvironmentVariable(key, previousValue);
        }
    }
}
