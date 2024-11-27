using Microsoft.AspNetCore.Builder;
using WebApi.SharedKernel.Middleware;

namespace WebApi.SharedKernel.Extensions;

public static class SecurityExtensions
{
    /// <summary>
    /// Extension method to use the <see cref="ApiKeyMiddleware"/> in the application. You can use this method to configure the middleware in the application.
    /// </summary>
    /// <param name="app">The application to use the middleware.</param>
    /// <param name="expectedApiKey">The expected api key that we want to receive in our header as <c>x-api-key</c>.</param>
    /// <remarks>Remember to provide the expected api key in the configuration file.</remarks>
    public static void UseApiKeyMiddleware(this WebApplication app, string? expectedApiKey)
    {
        if (!string.IsNullOrWhiteSpace(expectedApiKey))
        {
            app.UseMiddleware<ApiKeyMiddleware>(expectedApiKey);
        }
    }
}
