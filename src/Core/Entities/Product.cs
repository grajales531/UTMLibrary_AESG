namespace UtmMarket.Core.Entities;

/// <summary>
/// Entidad de dominio pura que representa un producto en el sistema.
/// Optimizado para .NET 10 y Native AOT con C# 14.
/// </summary>
public sealed class Product(int productId, string name, string sku, string? brand, decimal price = 0, int stock = 0)
{
    public int ProductID { get; init; } = productId;
    public string Name { get; set; } = name;
    public string SKU { get; init; } = sku;
    public string? Brand { get; set; } = brand;

    /// <summary>
    /// Precio unitario del producto. No puede ser negativo.
    /// Utiliza el backing field synthesis de C# 14 ('field').
    /// </summary>
    public decimal Price
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "El precio no puede ser negativo.");
            field = value;
        }
    } = price;

    /// <summary>
    /// Existencias actuales en inventario. No puede ser negativo.
    /// Utiliza el backing field synthesis de C# 14 ('field').
    /// </summary>
    public int Stock
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "El stock no puede ser negativo.");
            field = value;
        }
    } = stock;
}
