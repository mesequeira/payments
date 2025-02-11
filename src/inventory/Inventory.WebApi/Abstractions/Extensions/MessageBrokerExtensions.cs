using Inventory.WebApi.Stocks.Consumers;
using MassTransit;
using Microsoft.Extensions.Options;
using WebApi.SharedKernel.Options;

namespace Inventory.WebApi.Abstractions.Extensions;

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

            configurator.AddConsumer<ReserveStockEventConsumer>();
            
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