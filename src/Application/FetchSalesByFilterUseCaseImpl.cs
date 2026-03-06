namespace UtmMarket.Application;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Models;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to fetch sales by filter.
/// </summary>
public sealed class FetchSalesByFilterUseCaseImpl(ISaleRepository repository) : IFetchSalesByFilterUseCase
{
    private readonly ISaleRepository _repository = repository;

    public IAsyncEnumerable<Sale> ExecuteAsync(SaleFilter filter, CancellationToken ct = default)
    {
        return _repository.FindAsync(filter, ct);
    }
}
