using System.Net;
using DpopPortfolio.Functions.Shared.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace DpopPortfolio.Functions.Features.Identity;

public sealed class IdentityFunctions
{
    [Function(nameof(GetDemoUsers))]
    public static Task<HttpResponseData> GetDemoUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "identity/demo-users")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.OK,
            new
            {
                users = new[]
                {
                    new { username = "alex", displayName = "Alex Portfolio User", roles = new[] { "user" } },
                    new { username = "sam", displayName = "Sam Portfolio Admin", roles = new[] { "user", "admin" } }
                },
                note = "Passwords will be seeded as hashes when the identity milestone is implemented."
            });
    }

    [Function(nameof(Login))]
    public static Task<HttpResponseData> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "identity/login")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.NotImplemented,
            new
            {
                error = "identity_milestone_pending",
                message = "The next backend milestone will add demo credential validation, secure session cookies, and audit logging."
            });
    }

    [Function(nameof(Logout))]
    public static Task<HttpResponseData> Logout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "identity/logout")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.NotImplemented,
            new
            {
                error = "identity_milestone_pending",
                message = "Logout will clear the demo session cookie and write an audit event."
            });
    }
}
