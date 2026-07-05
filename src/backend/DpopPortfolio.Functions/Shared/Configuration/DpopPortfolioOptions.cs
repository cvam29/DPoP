namespace DpopPortfolio.Functions.Shared.Configuration;

public sealed class DpopPortfolioOptions
{
    public const string SectionName = "DpopPortfolio";

    public string Issuer { get; set; } = "http://localhost:7071";

    public string Audience { get; set; } = "dpop-portfolio-api";

    public string FrontendOrigin { get; set; } = "http://localhost:3000";

    public TokenLifetimeOptions TokenLifetimes { get; set; } = new();

    public string CacheProvider { get; set; } = "Memory";

    public string StorageProvider { get; set; } = "Azurite";

    public void ApplyEnvironmentFallbacks()
    {
        Issuer = GetEnvironmentString("DPOP_ISSUER", Issuer);
        Audience = GetEnvironmentString("DPOP_AUDIENCE", Audience);
        FrontendOrigin = GetEnvironmentString("DPOP_FRONTEND_ORIGIN", FrontendOrigin);
        CacheProvider = GetEnvironmentString("DPOP_CACHE_PROVIDER", CacheProvider);
        StorageProvider = GetEnvironmentString("DPOP_STORAGE_PROVIDER", StorageProvider);

        TokenLifetimes.AuthorizationCodeMinutes = GetEnvironmentInt(
            "DPOP_AUTHORIZATION_CODE_MINUTES",
            TokenLifetimes.AuthorizationCodeMinutes);
        TokenLifetimes.AccessTokenMinutes = GetEnvironmentInt(
            "DPOP_ACCESS_TOKEN_MINUTES",
            TokenLifetimes.AccessTokenMinutes);
        TokenLifetimes.RefreshTokenDays = GetEnvironmentInt(
            "DPOP_REFRESH_TOKEN_DAYS",
            TokenLifetimes.RefreshTokenDays);
        TokenLifetimes.DpopProofSeconds = GetEnvironmentInt(
            "DPOP_PROOF_SECONDS",
            TokenLifetimes.DpopProofSeconds);
    }

    private static string GetEnvironmentString(string key, string currentValue)
    {
        var value = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrWhiteSpace(value) ? currentValue : value.Trim();
    }

    private static int GetEnvironmentInt(string key, int currentValue)
    {
        var value = Environment.GetEnvironmentVariable(key);
        return int.TryParse(value, out var parsedValue) ? parsedValue : currentValue;
    }
}

public sealed class TokenLifetimeOptions
{
    public int AuthorizationCodeMinutes { get; set; } = 5;

    public int AccessTokenMinutes { get; set; } = 15;

    public int RefreshTokenDays { get; set; } = 7;

    public int DpopProofSeconds { get; set; } = 300;
}
