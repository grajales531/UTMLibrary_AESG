namespace UtmMarket.Core.UseCases;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Use Case for specifically updating the stock levels of a product.
/// </summary>
public interface IUpdateProductStockUseCase
{
    /// <summary>
    /// Executes an atomic update of the stock quantity.
    /// </summary>
    Task ExecuteAsync(int id, int newStock, CancellationToken ct = default);
}
