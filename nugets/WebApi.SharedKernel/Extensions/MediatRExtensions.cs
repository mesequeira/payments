using System.Reflection;
using Cross.SharedKernel.Behaviors;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.SharedKernel.Extensions;

public static class MediatRExtensions
{
    public static void AddMediatRConfiguration(
        this IServiceCollection services, 
        Assembly assembly,
        Action<MediatRServiceConfiguration>? externalConfiguration = null
    )
    {
        services.AddHttpContextAccessor();
        
        services.AddMediatR(configurator =>
        {
            configurator.RegisterServicesFromAssembly(assembly);
            configurator.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            configurator.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            configurator.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
            configurator.NotificationPublisher = new TaskWhenAllPublisher();
            externalConfiguration?.Invoke(configurator);
        });
    }
}