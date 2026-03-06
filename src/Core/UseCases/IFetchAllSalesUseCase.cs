namespace UtmMarket.Core.UseCases;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for retrieving all sales records in the system.
/// </summary>
public interface IFetchAllSalesUseCase
{
    /// <summary>
    /// Executes the retrieval of all sales as an asynchronous stream.
    /// </summary>
    IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken ct = default);
}
