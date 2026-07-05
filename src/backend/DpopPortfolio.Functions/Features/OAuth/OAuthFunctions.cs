using System.Net;
using DpopPortfolio.Functions.Shared.Configuration;
using DpopPortfolio.Functions.Shared.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace DpopPortfolio.Functions.Features.OAuth;

public sealed class OAuthFunctions(IOptions<DpopPortfolioOptions> options)
{
    [Function(nameof(GetAuthorizationServerMetadata))]
    public Task<HttpResponseData> GetAuthorizationServerMetadata(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".well-known/oauth-authorization-server")] HttpRequestData request)
    {
        var issuer = options.Value.Issuer.TrimEnd('/');

        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.OK,
            new
            {
                issuer,
                authorization_endpoint = $"{issuer}/oauth/authorize",
                token_endpoint = $"{issuer}/oauth/token",
                revocation_endpoint = $"{issuer}/oauth/revoke",
                jwks_uri = $"{issuer}/.well-known/jwks.json",
                audiences_supported = new[] { options.Value.Audience },
                response_types_supported = new[] { "code" },
                grant_types_supported = new[] { "authorization_code", "refresh_token" },
                code_challenge_methods_supported = new[] { "S256" },
                token_endpoint_auth_methods_supported = new[] { "none" },
                dpop_signing_alg_values_supported = new[] { "ES256" },
                scopes_supported = new[] { "openid", "profile", "todos.read", "todos.write", "admin.audit" }
            });
    }

    [Function(nameof(GetJwks))]
    public Task<HttpResponseData> GetJwks(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".well-known/jwks.json")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.OK,
            new
            {
                keys = Array.Empty<object>(),
                note = "Signing keys will be emitted once JWT access-token issuance is implemented."
            });
    }

    [Function(nameof(Authorize))]
    public Task<HttpResponseData> Authorize(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "oauth/authorize")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.NotImplemented,
            new
            {
                error = "oauth_milestone_pending",
                message = "Authorization Code + PKCE will validate client, redirect_uri, state, code_challenge, session, and requested scopes."
            });
    }

    [Function(nameof(Token))]
    public Task<HttpResponseData> Token(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "oauth/token")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.NotImplemented,
            new
            {
                error = "oauth_milestone_pending",
                message = "The token endpoint will require a DPoP proof and issue JWT access tokens with cnf.jkt binding."
            });
    }

    [Function(nameof(Revoke))]
    public Task<HttpResponseData> Revoke(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "oauth/revoke")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.NotImplemented,
            new
            {
                error = "oauth_milestone_pending",
                message = "Refresh-token revocation will remove the hashed token record and write an audit event."
            });
    }
}
