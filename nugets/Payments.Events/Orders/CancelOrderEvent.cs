namespace Payments.Events.Orders;

public record CancelOrderEvent(Guid TransactionId);