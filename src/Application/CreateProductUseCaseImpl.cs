namespace UtmMarket.Application;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to create a new product.
/// </summary>
public sealed class CreateProductUseCaseImpl(IProductRepository repository) : ICreateProductUseCase
{
    private readonly IProductRepository _repository = repository;

    public Task ExecuteAsync(Product product, CancellationToken ct = default)
    {
        // Simple business validation
        if (string.IsNullOrWhiteSpace(product.SKU))
            throw new ArgumentException("Product SKU cannot be empty.", nameof(product));

        return _repository.AddAsync(product, ct);
    }
}
