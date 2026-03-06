namespace UtmMarket.Core.UseCases;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Models;

/// <summary>
/// Use Case for fetching sales based on domain-specific filtering criteria.
/// </summary>
public interface IFetchSalesByFilterUseCase
{
    /// <summary>
    /// Executes the filtered search as an asynchronous stream.
    /// </summary>
    IAsyncEnumerable<Sale> ExecuteAsync(SaleFilter filter, CancellationToken ct = default);
}
