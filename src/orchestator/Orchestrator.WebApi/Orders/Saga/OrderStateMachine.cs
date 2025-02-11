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
        
        Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.TransactionId));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.TransactionId));

        
        // When the order was created, we transition to the OrderPlaced state and publish the CreatePaymentEvent.
        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(OrderPlaced)
                .Publish(context => new CreatePaymentEvent(context.Message.TransactionId)));
        
        During(OrderPlaced,
            When(PaymentCompleted)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(PaymentProcessed)
                // .Publish(context => new StockReservedEvent(context.Message.TransactionId))
                .Finalize());
    }
    
    public Event<OrderCreatedEvent>? OrderCreated { get; private set; }
    public Event<PaymentCompletedEvent>? PaymentCompleted { get; private set; }
    
    

    
    public State? OrderPlaced { get; private set; }
    public State? PaymentProcessed { get; private set; }
}