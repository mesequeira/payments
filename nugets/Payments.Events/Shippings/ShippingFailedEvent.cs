namespace Payments.Events.Shippings;

public record ShippingFailedEvent(Guid TransactionId);