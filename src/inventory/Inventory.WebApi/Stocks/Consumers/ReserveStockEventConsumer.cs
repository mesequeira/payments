using MassTransit;
using Payments.Events.Stocks;

namespace Inventory.WebApi.Stocks.Consumers;

internal sealed class ReserveStockEventConsumer : IConsumer<ReserveStockEvent>
{
    public Task Consume(ConsumeContext<ReserveStockEvent> context)
    {
        Console.WriteLine($"Reserve stock event received: {context.Message.TransactionId}");
        
        return Task.CompletedTask;
    }
}