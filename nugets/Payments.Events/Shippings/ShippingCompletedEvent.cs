namespace Payments.Events.Shippings;

public record ShippingCompletedEvent(Guid TransactionId);