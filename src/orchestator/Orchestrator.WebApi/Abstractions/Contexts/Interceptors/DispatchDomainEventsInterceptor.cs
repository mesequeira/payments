using Cross.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Orchestrator.WebApi.Abstractions.Contexts.Interceptors;

/// <summary>
/// Everytime a save operation is performed, this interceptor will dispatch one by one all the domain events of the entities.
/// </summary>
/// <param name="publisher">The publisher in charge to dispatch the domain events to be handled by the subscribers.</param>
public sealed class DispatchDomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var context = eventData.Context;

        if (context is null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        // Get all the entities that have domain events.
        var domainEvents = context
            .ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Count != 0)
            .SelectMany(e =>
            {
                var domainEvents = e.GetDomainEvents(); // Get the domain events.

                e.ClearDomainEvents(); // Clear the domain events.

                return domainEvents;
            })
            .ToList();

        // Iterate over the domain events and publish them.
        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        // Clear the domain events to avoid memory leaks or unexpected behavior.
        domainEvents.Clear();

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
