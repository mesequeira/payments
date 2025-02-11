using MassTransit;
using Payments.Events.Orders;

namespace Orchestrator.WebApi.Orders.Consumers;

public class CreateOrderEventConsumer : IConsumer<CreateOrderEvent>
{
    public async Task Consume(ConsumeContext<CreateOrderEvent> context)
    {
        await context.Publish(new OrderCreatedEvent(context.Message.TransactionId));
    }
}