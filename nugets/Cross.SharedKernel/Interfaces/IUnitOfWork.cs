using System.Data;

namespace Cross.SharedKernel.Interfaces;

/// <summary>
/// The interface that represents the unit of work pattern.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// It acts as a container for all the repositories and it is responsible for the actual persistence of data.
    /// Works as a interface between the repositories and the database.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An integer value representing the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a transaction on the underlying database connection.
    /// </summary>
    /// /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An object representing the transaction.</returns>
    Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
