namespace UtmMarket.Core.UseCases;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for orchestrating the creation and persistence of a new sale.
/// </summary>
public interface ICreateSaleUseCase
{
    /// <summary>
    /// Executes the sale creation process.
    /// </summary>
    /// <param name="sale">The new sale domain object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The persisted sale object with its assigned identity.</returns>
    Task<Sale> ExecuteAsync(Sale sale, CancellationToken ct = default);
}
