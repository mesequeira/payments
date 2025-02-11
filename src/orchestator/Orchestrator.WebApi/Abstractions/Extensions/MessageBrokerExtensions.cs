using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orchestrator.WebApi.Abstractions.Contexts;
using Orchestrator.WebApi.Abstractions.Options;
using Orchestrator.WebApi.Orders.Consumers;
using Orchestrator.WebApi.Orders.Saga;

namespace Orchestrator.WebApi.Abstractions.Extensions;

internal static class MessageBrokerExtensions
{
    public static void AddMessageBrokerConfiguration(
        this IServiceCollection services, 
        IConfiguration configuration
    )
    {
        services
            .AddOptionsWithValidateOnStart<MessageBrokerOptions>()
            .BindConfiguration(nameof(MessageBrokerOptions))
            .ValidateOnStart();

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();
            
            configurator.AddDelayedMessageScheduler();

            configurator.AddConsumer<CreateOrderEventConsumer>();

            configurator.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .EntityFrameworkRepository(sagaConfigurator =>
                {
                    sagaConfigurator.ExistingDbContext<ApplicationDbContext>();
                    sagaConfigurator.UseSqlServer();
                });
                
            
            configurator.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<MessageBrokerOptions>>().Value;
                
                cfg.UseDelayedMessageScheduler();
                
                cfg.ConfigureEndpoints(context);

                cfg.Host(options.ConnectionString);
            });
            
        });
    }
}