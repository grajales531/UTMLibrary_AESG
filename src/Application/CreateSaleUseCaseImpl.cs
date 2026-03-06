namespace UtmMarket.Application;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to create a new sale.
/// </summary>
public sealed class CreateSaleUseCaseImpl(ISaleRepository repository) : ICreateSaleUseCase
{
    private readonly ISaleRepository _repository = repository;

    public async Task<Sale> ExecuteAsync(Sale sale, CancellationToken ct = default)
    {
        // Business validation: Sale must have at least one detail
        if (sale.Details == null || sale.Details.Count == 0)
            throw new InvalidOperationException("Sale must have at least one detail.");

        // Additional business logic could go here (e.g., verifying stock for all products)
        
        return await _repository.AddAsync(sale, ct);
    }
}
