using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;

namespace Cross.SharedKernel.Abstractions;

/// <summary>
/// Interface for a generic repository with support for CRUD operations and pagination.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Gets a paginated list of DTOs based on the specified predicate, page index, and page size.
    /// </summary>
    /// <typeparam name="TDto">The type of the DTO.</typeparam>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <param name="pageIndex">The index of the page.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="configurationProvider">The AutoMapper configuration provider.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated result.</returns>
    Task<PaginatedResult<TDto>> GetAsync<TDto>(
        int pageIndex,
        int pageSize,
        IConfigurationProvider configurationProvider,
        CancellationToken cancellationToken = default,
        Expression<Func<TEntity, bool>>? predicate = null
    );

    /// <summary>
    /// Gets an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a DTO by the entity identifier asynchronously.
    /// </summary>
    /// <typeparam name="TDto">The type of the DTO.</typeparam>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="configurationProvider">The AutoMapper configuration provider.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the DTO.</returns>
    Task<TDto?> GetByIdProjectionAsync<TDto>(
        long id,
        IConfigurationProvider configurationProvider,
        CancellationToken cancellationToken = default
    );
}
