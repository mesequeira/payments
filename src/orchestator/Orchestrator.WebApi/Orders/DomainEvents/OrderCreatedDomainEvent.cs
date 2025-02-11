using Cross.SharedKernel.DomainEvents;

namespace Orchestrator.WebApi.Orders.DomainEvents;

public sealed record OrderCreatedDomainEvent(
    Guid TransactionId
) : IDomainEvent;