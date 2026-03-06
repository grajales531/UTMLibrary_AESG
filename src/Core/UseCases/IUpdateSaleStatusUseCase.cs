namespace UtmMarket.Core.UseCases;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for updating only the status of an existing sale.
/// </summary>
public interface IUpdateSaleStatusUseCase
{
    /// <summary>
    /// Executes the status update.
    /// </summary>
    Task ExecuteAsync(int saleId, SaleStatus newStatus, CancellationToken ct = default);
}
