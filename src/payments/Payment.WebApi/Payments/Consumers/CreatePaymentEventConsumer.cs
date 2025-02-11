using MassTransit;
using Payments.Events.Payments;

namespace Payment.WebApi.Payments.Consumers;

public class CreatePaymentEventConsumer(
    
) : IConsumer<CreatePaymentEvent>
{
    public Task Consume(ConsumeContext<CreatePaymentEvent> context)
    {
        Console.WriteLine($"Payment created for order {context.Message.TransactionId}");
        
        return Task.CompletedTask;
    }
}