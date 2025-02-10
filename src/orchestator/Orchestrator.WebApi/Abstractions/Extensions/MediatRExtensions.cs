using System.Reflection;
using Cross.SharedKernel.Behaviors;
using MediatR.NotificationPublishers;
using Orchestrator.WebApi.Idempotency.Behaviors;

namespace Orchestrator.WebApi.Abstractions.Extensions;

public static class MediatRExtensions
{
    public static void AddMediatRConfiguration(this IServiceCollection services, Assembly assembly)
    {
        services.AddHttpContextAccessor();
        
        services.AddMediatR(configurator =>
        {
            configurator.RegisterServicesFromAssembly(assembly);
            configurator.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            configurator.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            configurator.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
            configurator.AddOpenBehavior(typeof(IdempotentPipelineBehavior<,>));
            configurator.NotificationPublisher = new TaskWhenAllPublisher();
        });
    }
}