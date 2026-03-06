namespace UtmMarket.Infrastructure.Mappers;

using System;
using System.Collections.Generic;
using System.Linq;
using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Mapeador estático de alto rendimiento para la entidad Venta y sus detalles.
/// Optimizado para .NET 10 y Native AOT con C# 14.
/// </summary>
public static class SaleMapper
{
    /// <summary>
    /// Convierte una VentaEntity y sus detalles a un objeto de dominio Sale.
    /// </summary>
    /// <param name="entity">Entidad de persistencia VentaEntity.</param>
    /// <param name="details">Colección de detalles DetalleVentaEntity.</param>
    /// <param name="products">Diccionario de productos asociados para reconstruir el dominio.</param>
    /// <returns>Objeto de dominio Sale.</returns>
    public static Sale ToDomain(this VentaEntity entity, IEnumerable<DetalleVentaEntity> details, IDictionary<int, Product> products)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(products);

        var sale = new Sale(entity.VentaID, entity.Folio)
        {
            SaleDate = entity.FechaVenta,
            Status = (SaleStatus)entity.Estatus
        };

        foreach (var detailEntity in details)
        {
            if (products.TryGetValue(detailEntity.ProductoID, out var product))
            {
                var saleDetail = new SaleDetail(product, detailEntity.Cantidad);
                // Nota: UnitPrice en SaleDetail se hereda de product.Price por defecto en el constructor, 
                // pero si el histórico de la BD es diferente, aquí se podría ajustar si el dominio lo permite.
                sale.Details.Add(saleDetail);
            }
        }

        return sale;
    }

    /// <summary>
    /// Convierte un objeto de dominio Sale a su representación de persistencia VentaEntity.
    /// </summary>
    /// <param name="domain">Objeto de dominio Sale.</param>
    /// <returns>Entidad de persistencia VentaEntity.</returns>
    public static VentaEntity ToEntity(this Sale domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return new VentaEntity(domain.SaleID, domain.Folio)
        {
            FechaVenta = domain.SaleDate,
            Estatus = (byte)domain.Status,
            TotalArticulos = domain.TotalItems,
            TotalVenta = domain.TotalSale
        };
    }

    /// <summary>
    /// Convierte los detalles de una venta de dominio a entidades de persistencia.
    /// </summary>
    /// <param name="domain">Objeto de dominio Sale.</param>
    /// <returns>Colección de DetalleVentaEntity.</returns>
    public static IEnumerable<DetalleVentaEntity> ToDetailEntities(this Sale domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return domain.Details.Select(d => new DetalleVentaEntity(0, domain.SaleID, d.Product.ProductID)
        {
            PrecioUnitario = d.UnitPrice,
            Cantidad = d.Quantity,
            TotalDetalle = d.TotalDetail
        });
    }
}
