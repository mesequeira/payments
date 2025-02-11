using Cross.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Payment.WebApi.Abstractions.Contexts;
using Payment.WebApi.Abstractions.Contexts.Interceptors;

namespace Payment.WebApi.Abstractions.Extensions;

public static class PersistenceExtensions
{
    public static void AddPersistenceConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<UpdateAuditablePropsInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("OrchestatorConnectionString"));

            options.AddInterceptors(provider.GetRequiredService<UpdateAuditablePropsInterceptor>());
        });
    }
}