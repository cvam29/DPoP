using Microsoft.Azure.Functions.Worker.Http;

namespace DpopPortfolio.Functions.Shared.Http;

public static class CorsHeaders
{
    private const string AllowedHeaders = "Authorization, DPoP, Content-Type";
    private const string AllowedMethods = "GET, POST, PATCH, DELETE, OPTIONS";
    private const string ExposedHeaders = "WWW-Authenticate, X-RateLimit-Limit, X-RateLimit-Remaining, X-RateLimit-Reset, Retry-After";

    public static void Apply(HttpResponseData response, string frontendOrigin)
    {
        response.Headers.Add("Access-Control-Allow-Origin", frontendOrigin);
        response.Headers.Add("Access-Control-Allow-Credentials", "true");
        response.Headers.Add("Access-Control-Allow-Headers", AllowedHeaders);
        response.Headers.Add("Access-Control-Allow-Methods", AllowedMethods);
        response.Headers.Add("Access-Control-Expose-Headers", ExposedHeaders);
        response.Headers.Add("Vary", "Origin");
    }
}
