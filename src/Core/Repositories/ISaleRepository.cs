namespace UtmMarket.Core.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Models;

/// <summary>
/// Repository contract for the 'Sale' aggregate root.
/// Designed for high-performance streaming and Native AOT compatibility.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Retrieves all sales as an asynchronous stream to optimize memory usage.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>An IAsyncEnumerable of Sale domain objects.</returns>
    IAsyncEnumerable<Sale> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific sale by its unique database identifier, including all its details.
    /// </summary>
    /// <param name="id">The unique ID of the sale.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The Sale domain object if found; otherwise, null.</returns>
    Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds sales based on specific filtering criteria.
    /// </summary>
    /// <param name="filter">The search criteria encapsulated in a SaleFilter object.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>An IAsyncEnumerable of Sale domain objects matching the criteria.</returns>
    IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new sale aggregate into the database.
    /// </summary>
    /// <param name="sale">The Sale domain object to persist.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The persisted Sale object with its generated identity.</returns>
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale aggregate and its associated details.
    /// </summary>
    /// <param name="sale">The Sale domain object with updated values.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
}
