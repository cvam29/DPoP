using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;

namespace DpopPortfolio.Functions.Shared.Http;

public static class JsonResponse
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static async Task<HttpResponseData> CreateAsync<T>(
        HttpRequestData request,
        HttpStatusCode statusCode,
        T body)
    {
        var response = request.CreateResponse(statusCode);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");

        await response.WriteStringAsync(JsonSerializer.Serialize(body, SerializerOptions));

        return response;
    }
}
