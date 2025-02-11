namespace Payments.Events.Shippings;

public record ShippingPreparedEvent(Guid TransactionId);