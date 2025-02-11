namespace Payments.Events.Orders;

public record OrderCreatedEvent(Guid TransactionId);