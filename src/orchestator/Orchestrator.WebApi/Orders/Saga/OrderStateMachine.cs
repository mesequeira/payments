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

        // Definición de eventos
        Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.TransactionId));
        Event(() => StockReserved, x => x.CorrelateById(m => m.Message.TransactionId));
        Event(() => StockReservationFailed, x => x.CorrelateById(m => m.Message.TransactionId));
        Event(() => ShippingPrepared, x => x.CorrelateById(m => m.Message.TransactionId));
        Event(() => ShippingFailed, x => x.CorrelateById(m => m.Message.TransactionId));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.TransactionId));
        Event(() => PaymentFailed, x => x.CorrelateById(m => m.Message.TransactionId));

        // Estado inicial: Orden creada, se publica el evento para reservar stock
        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(OrderPlaced)
                .Publish(context => new ReserveStockEvent(context.Message.TransactionId))
        );

        // Estado: Orden colocada y esperando la reserva de stock
        During(OrderPlaced,
            When(StockReserved)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(StockReservedState)
                .Publish(context => new PrepareShippingEvent(context.Message.TransactionId)),

            When(StockReservationFailed)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(StockFailedState)
                .Publish(context => new CancelOrderEvent(context.Message.TransactionId))
        );

        // Estado: Stock reservado, esperando preparación de envío
        During(StockReservedState,
            When(ShippingPrepared)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(ShippingPreparedState)
                .Publish(context => new ProcessPaymentEvent(context.Message.TransactionId)),

            When(ShippingFailed)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(ShippingFailedState)
                .Publish(context => new ReleaseStockEvent(context.Message.TransactionId))
        );

        // Estado: Envío preparado, esperando confirmación de pago
        During(ShippingPreparedState,
            When(PaymentCompleted)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(OrderCompleted)
                .Finalize(),

            When(PaymentFailed)
                .Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                })
                .TransitionTo(PaymentFailedState)
                .Publish(context => new CancelShippingEvent(context.Message.TransactionId))
        );

        // Definir los estados finales y posibles compensaciones
        SetCompletedWhenFinalized();
    }

    // Eventos
    public Event<OrderCreatedEvent>? OrderCreated { get; private set; }
    public Event<StockReservedEvent>? StockReserved { get; private set; }
    public Event<StockReservationFailedEvent>? StockReservationFailed { get; private set; }
    public Event<ShippingPreparedEvent>? ShippingPrepared { get; private set; }
    public Event<ShippingFailedEvent>? ShippingFailed { get; private set; }
    public Event<PaymentCompletedEvent>? PaymentCompleted { get; private set; }
    public Event<PaymentFailedEvent>? PaymentFailed { get; private set; }

    // Estados
    public State? OrderPlaced { get; private set; }
    public State? StockReservedState { get; private set; }
    public State? StockFailedState { get; private set; }
    public State? ShippingPreparedState { get; private set; }
    public State? ShippingFailedState { get; private set; }
    public State? PaymentFailedState { get; private set; }
    public State? OrderCompleted { get; private set; }
}
