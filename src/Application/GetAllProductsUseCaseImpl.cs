namespace UtmMarket.Application;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to retrieve all products.
/// </summary>
public sealed class GetAllProductsUseCaseImpl(IProductRepository repository) : IGetAllProductsUseCase
{
    private readonly IProductRepository _repository = repository;

    public IAsyncEnumerable<Product> ExecuteAsync(CancellationToken ct = default)
    {
        return _repository.GetAllAsync(ct);
    }
}
