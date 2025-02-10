using Microsoft.EntityFrameworkCore;
using Orchestrator.WebApi.Abstractions.Contexts;
using Orchestrator.WebApi.Idempotency.Entities;

namespace Orchestrator.WebApi.Idempotency.Repositories;

internal sealed class IdempotentRequestRepository(
    ApplicationDbContext context    
) : IIdempotentRequestRepository
{
    public async Task<bool> ExistsAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        return await context.IdempotentRequests.AnyAsync(
            idempotentRequest => idempotentRequest.RequestId == requestId, 
            cancellationToken: cancellationToken
        );
    }

    public async Task CreateAsync(Guid requestId, string name, CancellationToken cancellationToken = default)
    {
        var idempotentRequest = new IdempotentRequest
        {
            RequestId = requestId,
            Name = name
        };
        
        await context.IdempotentRequests.AddAsync(idempotentRequest, cancellationToken);
    }
}