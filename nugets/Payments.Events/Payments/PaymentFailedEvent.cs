namespace Payments.Events.Payments;

public record PaymentFailedEvent(Guid OrderId);