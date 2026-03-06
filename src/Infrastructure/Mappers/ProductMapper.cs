namespace UtmMarket.Infrastructure.Mappers;

using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Mapeador estático de alto rendimiento para la entidad Producto.
/// Diseñado para compatibilidad con Native AOT y C# 14.
/// </summary>
public static class ProductMapper
{
    /// <summary>
    /// Convierte una entidad de base de datos a un objeto de dominio.
    /// </summary>
    /// <param name="entity">Entidad de persistencia ProductoEntity.</param>
    /// <returns>Objeto de dominio Product.</returns>
    public static Product ToDomain(this ProductoEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        return new Product(
            productId: entity.ProductoID,
            name: entity.Nombre,
            sku: entity.SKU,
            brand: entity.Marca,
            price: entity.Precio,
            stock: entity.Stock
        );
    }

    /// <summary>
    /// Convierte un objeto de dominio a una entidad de base de datos.
    /// </summary>
    /// <param name="domain">Objeto de dominio Product.</param>
    /// <returns>Entidad de persistencia ProductoEntity.</returns>
    public static ProductoEntity ToEntity(this Product domain)
    {
        ArgumentNullException.ThrowIfNull(domain);
        
        return new ProductoEntity(
            productoId: domain.ProductID,
            sku: domain.SKU
        )
        {
            Nombre = domain.Name,
            Marca = domain.Brand,
            Precio = domain.Price,
            Stock = domain.Stock
        };
    }
}
