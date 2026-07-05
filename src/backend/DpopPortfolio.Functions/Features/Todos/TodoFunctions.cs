using System.Net;
using DpopPortfolio.Functions.Shared.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace DpopPortfolio.Functions.Features.Todos;

public sealed class TodoFunctions
{
    [Function(nameof(GetTodos))]
    public static async Task<HttpResponseData> GetTodos(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/todos")] HttpRequestData request)
    {
        if (!HasDpopAuthorization(request))
        {
            var unauthorized = await JsonResponse.CreateAsync(
                request,
                HttpStatusCode.Unauthorized,
                new
                {
                    error = "dpop_token_required",
                    message = "Protected resources require Authorization: DPoP <access_token> and a DPoP proof JWT."
                });

            unauthorized.Headers.Add("WWW-Authenticate", "DPoP error=\"invalid_token\", error_description=\"DPoP access token required\"");
            return unauthorized;
        }

        return await JsonResponse.CreateAsync(
            request,
            HttpStatusCode.NotImplemented,
            new
            {
                error = "resource_authorization_milestone_pending",
                message = "The Todo API will validate access token signature, cnf.jkt binding, scopes, and user ownership before returning data."
            });
    }

    [Function(nameof(CreateTodo))]
    public static Task<HttpResponseData> CreateTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/todos")] HttpRequestData request)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.Unauthorized,
            new
            {
                error = "dpop_token_required",
                message = "Creating todos will require a valid DPoP-bound token with the todos.write scope."
            });
    }

    [Function(nameof(UpdateTodo))]
    public static Task<HttpResponseData> UpdateTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "api/todos/{id}")] HttpRequestData request,
        string id)
    {
        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.Unauthorized,
            new
            {
                error = "dpop_token_required",
                message = $"Updating todo '{id}' will require ownership plus the todos.write scope."
            });
    }

    private static bool HasDpopAuthorization(HttpRequestData request)
    {
        return request.Headers.TryGetValues("Authorization", out var authorizationValues)
            && authorizationValues.Any(value => value.StartsWith("DPoP ", StringComparison.OrdinalIgnoreCase))
            && request.Headers.TryGetValues("DPoP", out var proofValues)
            && proofValues.Any(value => !string.IsNullOrWhiteSpace(value));
    }
}
