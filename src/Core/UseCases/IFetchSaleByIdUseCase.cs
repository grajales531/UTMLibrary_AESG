namespace UtmMarket.Core.UseCases;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for retrieving a specific sale by its unique identifier.
/// </summary>
public interface IFetchSaleByIdUseCase
{
    /// <summary>
    /// Executes the search for a sale by its ID.
    /// </summary>
    /// <returns>The sale domain object if found, otherwise null.</returns>
    Task<Sale?> ExecuteAsync(int id, CancellationToken ct = default);
}
