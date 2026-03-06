namespace UtmMarket.Application;

using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

/// <summary>
/// Implementation of the use case to update a sale's status.
/// </summary>
public sealed class UpdateSaleStatusUseCaseImpl(ISaleRepository repository) : IUpdateSaleStatusUseCase
{
    private readonly ISaleRepository _repository = repository;

    public async Task ExecuteAsync(int saleId, SaleStatus newStatus, CancellationToken ct = default)
    {
        var existingSale = await _repository.GetByIdAsync(saleId, ct);
        if (existingSale == null)
            throw new InvalidOperationException($"Sale with ID {saleId} not found.");

        // Simple update logic
        existingSale.Status = newStatus;
        
        await _repository.UpdateAsync(existingSale, ct);
    }
}
