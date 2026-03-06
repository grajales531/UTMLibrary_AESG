namespace UtmMarket.Application;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to fetch a sale by its ID.
/// </summary>
public sealed class FetchSaleByIdUseCaseImpl(ISaleRepository repository) : IFetchSaleByIdUseCase
{
    private readonly ISaleRepository _repository = repository;

    public Task<Sale?> ExecuteAsync(int id, CancellationToken ct = default)
    {
        return _repository.GetByIdAsync(id, ct);
    }
}
