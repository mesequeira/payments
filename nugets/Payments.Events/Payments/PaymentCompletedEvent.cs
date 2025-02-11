namespace Payments.Events.Payments;

public record PaymentCompletedEvent(Guid TransactionId);