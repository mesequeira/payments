using Cross.SharedKernel.Entities;

namespace Orchestrator.WebApi.Idempotency.Entities;

public class IdempotentRequest : Entity
{
    public Guid RequestId { get; set; }

    public string Name { get; set; } = default!;
}