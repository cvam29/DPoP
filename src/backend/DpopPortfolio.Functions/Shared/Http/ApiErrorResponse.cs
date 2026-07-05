namespace DpopPortfolio.Functions.Shared.Http;

public sealed record ApiErrorResponse(
    string Error,
    string Message,
    string TraceId);
