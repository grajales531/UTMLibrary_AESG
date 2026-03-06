namespace UtmMarket.Application;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to update a product's stock.
/// </summary>
public sealed class UpdateProductStockUseCaseImpl(IProductRepository repository) : IUpdateProductStockUseCase
{
    private readonly IProductRepository _repository = repository;

    public async Task ExecuteAsync(int id, int newStock, CancellationToken ct = default)
    {
        if (newStock < 0)
            throw new ArgumentOutOfRangeException(nameof(newStock), "Stock cannot be negative.");

        await _repository.UpdateStockAsync(id, newStock, ct);
    }
}
