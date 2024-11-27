using Microsoft.AspNetCore.Http;

namespace WebApi.SharedKernel.Middleware;

internal sealed class ApiKeyMiddleware(string expectedApiKey, RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if (
            !context.Request.Headers.TryGetValue("x-api-key", out var apiKey)
            || string.IsNullOrWhiteSpace(apiKey)
            || !string.Equals(apiKey, expectedApiKey)
        )
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(context);
    }
}
