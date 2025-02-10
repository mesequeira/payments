﻿using Cross.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Orchestrator.WebApi.Abstractions.Contexts;
using Orchestrator.WebApi.Abstractions.Contexts.Interceptors;
using Orchestrator.WebApi.Idempotency.Repositories;

namespace Orchestrator.WebApi.Abstractions.Extensions;

public static class PersistenceExtensions
{
    public static void AddPersistenceConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IIdempotentRequestRepository, IdempotentRequestRepository>();
        services.AddSingleton<UpdateAuditablePropsInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("OrchestatorConnectionString"));

            options.AddInterceptors(provider.GetRequiredService<UpdateAuditablePropsInterceptor>());
        });
    }
}