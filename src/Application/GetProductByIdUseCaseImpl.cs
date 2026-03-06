namespace UtmMarket.Application;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to retrieve a product by its ID.
/// </summary>
public sealed class GetProductByIdUseCaseImpl(IProductRepository repository) : IGetProductByIdUseCase
{
    private readonly IProductRepository _repository = repository;

    public Task<Product?> ExecuteAsync(int id, CancellationToken ct = default)
    {
        return _repository.GetByIdAsync(id, ct);
    }
}
