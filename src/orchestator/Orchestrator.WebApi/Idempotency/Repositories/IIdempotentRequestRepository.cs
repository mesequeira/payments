namespace Orchestrator.WebApi.Idempotency.Repositories;

public interface IIdempotentRequestRepository
{
    Task<bool> ExistsAsync(Guid requestId, CancellationToken cancellationToken = default);

    Task CreateAsync(Guid requestId, string name, CancellationToken cancellationToken = default);
}