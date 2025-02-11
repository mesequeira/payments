using Cross.SharedKernel.Entities;
using Orchestrator.WebApi.Orders.DomainEvents;

namespace Orchestrator.WebApi.Orders.Entities;

public sealed class Order : Entity
{
    public Guid TransactionId { get; set; }

    public decimal Amount { get; set; }
    
    public static Order Create(
        decimal amount,
        Guid transactionId
    )
    {
        var order = new Order
        {
            TransactionId = transactionId,
            Amount = amount
        };
        
        order.Raise(new OrderCreatedDomainEvent(transactionId));

        return order;
    }
}