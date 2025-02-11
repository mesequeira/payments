using Orchestrator.WebApi.Abstractions.Contexts;
using Orchestrator.WebApi.Orders.Entities;

namespace Orchestrator.WebApi.Orders.Repositories;

internal sealed class OrderRepository(
    ApplicationDbContext context    
) : IOrderRepository 
{
    public async Task CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await context.Orders.AddAsync(order, cancellationToken);
    }
}