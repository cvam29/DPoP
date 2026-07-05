using System.Net;
using DpopPortfolio.Functions.Shared.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace DpopPortfolio.Functions.Features.Admin;

public sealed class AdminAuditFunctions
{
    [Function(nameof(GetAuditEvents))]
    public static Task<HttpResponseData> GetAuditEvents(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/admin/audit")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.Unauthorized,
            new
            {
                error = "admin_authorization_required",
                message = "This endpoint is reserved for the admin role and will require a DPoP-bound access token with an admin role claim.",
                required = new[] { "Authorization: DPoP <access_token>", "DPoP: <proof_jwt>", "role: admin" }
            });
    }
}
