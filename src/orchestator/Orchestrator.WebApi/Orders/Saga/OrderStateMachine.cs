using MassTransit;
using Payments.Events.Orders;
using Payments.Events.Payments;
using Payments.Events.Shippings;
using Payments.Events.Stocks;

namespace Orchestrator.WebApi.Orders.Saga;

internal sealed class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public OrderStateMachine()
    {
        InstanceState(orderState => orderState.CurrentState);
        
        Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentFailed, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReserved, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReservationFailed, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => ShippingCompleted, x => x.CorrelateById(m => m.Message.OrderId));

        
        SetCompletedWhenFinalized();
    }
    
    public Event<OrderCreatedEvent>? OrderCreated { get; private set; }
    public Event<PaymentCompletedEvent>? PaymentCompleted { get; private set; }
    public Event<PaymentFailedEvent>? PaymentFailed { get; private set; }
    public Event<StockReservedEvent>? StockReserved { get; private set; }
    public Event<StockReservationFailedEvent>? StockReservationFailed { get; private set; }
    public Event<ShippingCompletedEvent>? ShippingCompleted { get; private set; }

    
    public State? OrderPlaced { get; private set; }
    public State? PaymentProcessed { get; private set; }
    public State? StockConfirmed { get; private set; }
    public State? Compensating { get; private set; }
}