using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.SharedKernel.Options;

/// <summary>
/// Represents the configuration for the swagger like setting the title and version.
/// </summary>
/// <param name="provider">The provider for the API version description.</param>
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = $"{nameof(WebApi)} v{description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                }
            );
        }
    }
}
