using Cross.SharedKernel.Interfaces;
using Inventory.WebApi.Abstractions.Contexts;
using Inventory.WebApi.Abstractions.Contexts.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Inventory.WebApi.Abstractions.Extensions;

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
            options.UseSqlServer(configuration.GetConnectionString("InventoryConnectionString"));

            options.AddInterceptors(provider.GetRequiredService<UpdateAuditablePropsInterceptor>());
        });
    }
}