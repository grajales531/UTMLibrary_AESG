namespace UtmMarket.Application;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to update an existing product.
/// </summary>
public sealed class UpdateProductUseCaseImpl(IProductRepository repository) : IUpdateProductUseCase
{
    private readonly IProductRepository _repository = repository;

    public async Task ExecuteAsync(Product product, CancellationToken ct = default)
    {
        // Check if the product exists before updating
        var existingProduct = await _repository.GetByIdAsync(product.ProductID, ct);
        if (existingProduct == null)
            throw new InvalidOperationException($"Product with ID {product.ProductID} not found.");

        await _repository.UpdateAsync(product, ct);
    }
}
