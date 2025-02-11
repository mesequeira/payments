namespace Payments.Events.Payments;

public record ProcessPaymentEvent(Guid TransactionId);