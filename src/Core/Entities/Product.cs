namespace UtmMarket.Core.Entities;

/// <summary>
/// Entidad de dominio pura que representa un producto en el sistema.
/// Optimizado para .NET 10 y Native AOT.
/// </summary>
public sealed class Product(int productId, string name, string sku, string? brand)
{
    public int ProductID { get; init; } = productId;
    public string Name { get; set; } = name;
    public string SKU { get; init; } = sku;
    public string? Brand { get; set; } = brand;

    /// <summary>
    /// Precio unitario del producto. No puede ser negativo.
    /// </summary>
    public decimal Price
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentException("El precio no puede ser negativo.");
            field = value;
        }
    }

    /// <summary>
    /// Existencias actuales en inventario. No puede ser negativo.
    /// </summary>
    public int Stock
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentException("El stock no puede ser negativo.");
            field = value;
        }
    }
}
