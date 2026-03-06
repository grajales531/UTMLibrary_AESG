namespace UtmMarket.Core.UseCases;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for updating an existing product's information.
/// </summary>
public interface IUpdateProductUseCase
{
    /// <summary>
    /// Executes the update of the product data.
    /// </summary>
    Task ExecuteAsync(Product product, CancellationToken ct = default);
}
