namespace UtmMarket.Application;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Models;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to find products based on filters.
/// </summary>
public sealed class FindProductsUseCaseImpl(IProductRepository repository) : IFindProductsUseCase
{
    private readonly IProductRepository _repository = repository;

    public IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken ct = default)
    {
        return _repository.FindAsync(filter, ct);
    }
}
