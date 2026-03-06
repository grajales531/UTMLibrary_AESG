namespace UtmMarket.Core.UseCases;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Models;

/// <summary>
/// Use Case for finding products based on specific criteria.
/// </summary>
public interface IFindProductsUseCase
{
    /// <summary>
    /// Executes the filtered search as an asynchronous stream.
    /// </summary>
    IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken ct = default);
}
