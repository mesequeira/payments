namespace Payments.Events.Shippings;

public record CancelShippingEvent(Guid TransactionId);