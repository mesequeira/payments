using Orchestrator.WebApi.Orders.Entities;

namespace Orchestrator.WebApi.Orders.Repositories;

public interface IOrderRepository 
{
    Task CreateAsync(Order order, CancellationToken cancellationToken = default);
}