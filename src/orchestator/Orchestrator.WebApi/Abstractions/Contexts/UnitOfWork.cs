using System.Data;
using Cross.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Orchestrator.WebApi.Abstractions.Contexts;

internal sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    )
    {
        return (await context.Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }
}
