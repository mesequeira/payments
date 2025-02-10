namespace Payments.Events.Orders;

public record OrderCreatedEvent(double Amount, Guid OrderId);