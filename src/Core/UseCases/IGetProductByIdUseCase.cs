namespace UtmMarket.Core.UseCases;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

/// <summary>
/// Use Case for retrieving a single product by its unique identifier.
/// </summary>
public interface IGetProductByIdUseCase
{
    /// <summary>
    /// Executes the search for a product.
    /// </summary>
    /// <returns>The product domain object if found, otherwise null.</returns>
    Task<Product?> ExecuteAsync(int id, CancellationToken ct = default);
}
