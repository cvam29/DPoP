using Microsoft.Extensions.Options;

namespace DpopPortfolio.Functions.Shared.Configuration;

public sealed class DpopPortfolioOptionsValidator : IValidateOptions<DpopPortfolioOptions>
{
    private static readonly HashSet<string> CacheProviders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Memory",
        "Redis"
    };

    private static readonly HashSet<string> StorageProviders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Memory",
        "Azurite",
        "AzureTable",
        "AzureStorage"
    };

    public ValidateOptionsResult Validate(string? name, DpopPortfolioOptions options)
    {
        var errors = new List<string>();

        ValidateUri(options.Issuer, $"{DpopPortfolioOptions.SectionName}:Issuer", requireHttpsOutsideLocalhost: true, errors);
        ValidateUri(options.FrontendOrigin, $"{DpopPortfolioOptions.SectionName}:FrontendOrigin", requireHttpsOutsideLocalhost: true, errors);

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            errors.Add($"{DpopPortfolioOptions.SectionName}:Audience is required.");
        }

        if (!CacheProviders.Contains(options.CacheProvider))
        {
            errors.Add($"{DpopPortfolioOptions.SectionName}:CacheProvider must be one of: {string.Join(", ", CacheProviders)}.");
        }

        if (!StorageProviders.Contains(options.StorageProvider))
        {
            errors.Add($"{DpopPortfolioOptions.SectionName}:StorageProvider must be one of: {string.Join(", ", StorageProviders)}.");
        }

        ValidateRange(options.TokenLifetimes.AuthorizationCodeMinutes, 1, 15, "AuthorizationCodeMinutes", errors);
        ValidateRange(options.TokenLifetimes.AccessTokenMinutes, 1, 60, "AccessTokenMinutes", errors);
        ValidateRange(options.TokenLifetimes.RefreshTokenDays, 1, 30, "RefreshTokenDays", errors);
        ValidateRange(options.TokenLifetimes.DpopProofSeconds, 30, 600, "DpopProofSeconds", errors);

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }

    private static void ValidateUri(
        string value,
        string key,
        bool requireHttpsOutsideLocalhost,
        ICollection<string> errors)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            errors.Add($"{key} must be an absolute URI.");
            return;
        }

        if (requireHttpsOutsideLocalhost
            && uri.Scheme != Uri.UriSchemeHttps
            && !IsLocalDevelopmentHost(uri))
        {
            errors.Add($"{key} must use HTTPS outside local development.");
        }
    }

    private static bool IsLocalDevelopmentHost(Uri uri)
    {
        return uri.IsLoopback
            || uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase);
    }

    private static void ValidateRange(
        int value,
        int minimum,
        int maximum,
        string settingName,
        ICollection<string> errors)
    {
        if (value < minimum || value > maximum)
        {
            errors.Add(
                $"{DpopPortfolioOptions.SectionName}:TokenLifetimes:{settingName} must be between {minimum} and {maximum}.");
        }
    }
}
