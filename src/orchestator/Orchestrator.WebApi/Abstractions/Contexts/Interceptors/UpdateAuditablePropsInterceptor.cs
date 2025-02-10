using Cross.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Orchestrator.WebApi.Abstractions.Contexts.Interceptors;

public sealed class UpdateAuditablePropsInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableProps(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableProps(DbContext context)
    {
        var entities = context
                        .ChangeTracker
                        .Entries<Entity>();

        var utcNow = DateTime.UtcNow;

        foreach (EntityEntry<Entity> entityEntry in entities)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedOnUtc = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Entity.ModifiedOnUtc = utcNow;
            }
        }
    }
}
