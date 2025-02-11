namespace Payments.Events.Orders;

public record CreateOrderEvent(Guid TransactionId, decimal Amount);