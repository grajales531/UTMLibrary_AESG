namespace UtmMarket.Core.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Models;

/// <summary>
/// Contrato de repositorio para la entidad de dominio Product.
/// Diseñado para alto rendimiento y compatibilidad con Native AOT.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Obtiene todos los productos del catálogo.
    /// Utiliza IAsyncEnumerable para streaming de datos eficiente.
    /// </summary>
    IAsyncEnumerable<Product> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene un producto por su identificador único de base de datos.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>El producto si existe, de lo contrario null.</returns>
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Busca productos basándose en criterios de filtrado específicos.
    /// </summary>
    /// <param name="filter">Objeto de criterios ProductFilter.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Flujo asíncrono de productos que coinciden con el filtro.</returns>
    IAsyncEnumerable<Product> FindAsync(ProductFilter filter, CancellationToken ct = default);

    /// <summary>
    /// Registra un nuevo producto en la persistencia.
    /// </summary>
    Task AddAsync(Product product, CancellationToken ct = default);

    /// <summary>
    /// Actualiza la información de un producto existente.
    /// </summary>
    Task UpdateAsync(Product product, CancellationToken ct = default);

    /// <summary>
    /// Realiza una actualización atómica parcial únicamente del stock de un producto.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <param name="newStock">Cantidad actualizada de existencias.</param>
    /// <param name="ct">Token de cancelación.</param>
    Task UpdateStockAsync(int id, int newStock, CancellationToken ct = default);

    /// <summary>
    /// Elimina un producto de la persistencia por su ID.
    /// </summary>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
