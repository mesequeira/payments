using Microsoft.Extensions.DependencyInjection;

namespace Cross.SharedKernel.Extensions;

public static class InjectExtensions
{
    /// <summary>
    /// Inject all the services of the specified type from the current assembly with a scoped lifetime.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <typeparam name="TService">The type of the services to add.</typeparam>
    public static void ImplementWithScopedLifeTime<TService>(this IServiceCollection services)
    {
        services.Scan(selector =>
            selector
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(TService)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }

    /// <summary>
    /// Inject all the services of the specified type from the current assembly with a singleton lifetime.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <typeparam name="TService">The type of the services to add.</typeparam>
    public static void ImplementedWithSingletonLifeTime<TService>(this IServiceCollection services)
    {
        services.Scan(selector =>
            selector
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(TService)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
        );
    }

    /// <summary>
    /// Inject all the services of the specified type from the current assembly with a transientQ lifetime.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <typeparam name="TService">The type of the services to add.</typeparam>
    public static void ImplementWithTransientLifeTime<TService>(this IServiceCollection services)
    {
        services.Scan(selector =>
            selector
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(TService)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
    }
}
