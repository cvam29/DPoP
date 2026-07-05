using System.Net;
using DpopPortfolio.Functions.Shared.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace DpopPortfolio.Functions.Features.Dpop;

public sealed class DpopConceptFunctions
{
    [Function(nameof(GetDpopConcepts))]
    public static Task<HttpResponseData> GetDpopConcepts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/dpop/concepts")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.OK,
            new
            {
                standard = "RFC 9449",
                proofHeader = "DPoP",
                tokenScheme = "DPoP",
                proofClaims = new[] { "htu", "htm", "iat", "jti", "ath" },
                bindingClaim = "cnf.jkt",
                replayProtection = "Cache each proof jti until the accepted proof window expires.",
                upcomingImplementation = new[]
                {
                    "Validate proof JWT signature against the embedded public JWK.",
                    "Compute the JWK thumbprint and compare it with access token cnf.jkt.",
                    "Reject reused jti values through Redis-backed cache storage.",
                    "Validate ath against the presented DPoP access token."
                }
            });
    }
}
