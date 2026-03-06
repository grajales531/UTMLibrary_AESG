namespace UtmMarket.Core.UseCases;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for creating a new product in the catalog.
/// </summary>
public interface ICreateProductUseCase
{
    /// <summary>
    /// Executes the creation of the product.
    /// </summary>
    Task ExecuteAsync(Product product, CancellationToken ct = default);
}
