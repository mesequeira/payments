using MassTransit;
using Microsoft.Extensions.Options;
using Payment.WebApi.Abstractions.Options;
using Payment.WebApi.Payments.Consumers;

namespace Payment.WebApi.Abstractions.Extensions;

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

            configurator.AddConsumer<CreatePaymentEventConsumer>();
            
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