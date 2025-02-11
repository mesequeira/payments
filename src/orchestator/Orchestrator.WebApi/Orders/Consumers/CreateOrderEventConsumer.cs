using Cross.SharedKernel.Interfaces;
using MassTransit;
using Orchestrator.WebApi.Orders.Entities;
using Orchestrator.WebApi.Orders.Repositories;
using Payments.Events.Orders;

namespace Orchestrator.WebApi.Orders.Consumers;

public class CreateOrderEventConsumer(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork
) : IConsumer<CreateOrderEvent>
{
    public async Task Consume(ConsumeContext<CreateOrderEvent> context)
    {

        Order order = Order.Create(
            context.Message.Amount,
            context.Message.TransactionId
        );
        
        await orderRepository.CreateAsync(order, context.CancellationToken);
        
        await unitOfWork.SaveChangesAsync(context.CancellationToken);
        
        await context.Publish(new OrderCreatedEvent(context.Message.TransactionId));
    }
}