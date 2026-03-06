namespace UtmMarket.Application;

using System.Collections.Generic;
using System.Threading;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to fetch all sales.
/// </summary>
public sealed class FetchAllSalesUseCaseImpl(ISaleRepository repository) : IFetchAllSalesUseCase
{
    private readonly ISaleRepository _repository = repository;

    public IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken ct = default)
    {
        return _repository.GetAllAsync(ct);
    }
}
