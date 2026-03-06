namespace UtmMarket.Core.UseCases;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for retrieving all products in the system.
/// </summary>
public interface IGetAllProductsUseCase
{
    /// <summary>
    /// Executes the retrieval of all products as an asynchronous stream.
    /// </summary>
    IAsyncEnumerable<Product> ExecuteAsync(CancellationToken ct = default);
}
