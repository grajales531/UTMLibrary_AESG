namespace UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Modelo de datos parcial para la tabla [dbo].[Producto].
/// Compatible con Native AOT y optimizado para el mapeo manual.
/// </summary>
public partial class ProductoEntity(int productoId, string sku)
{
    public int ProductoID { get; init; } = productoId;
    public string SKU { get; init; } = sku;
    public string Nombre { get; set; } = string.Empty;
    public string? Marca { get; set; }

    /// <summary>
    /// Precio unitario del producto (DECIMAL 19,4).
    /// Validación vía C# 14 field keyword.
    /// </summary>
    public decimal Precio
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "El precio debe ser mayor o igual a 0.");
            field = value;
        }
    }

    /// <summary>
    /// Stock actual en inventario (INT).
    /// Validación vía C# 14 field keyword.
    /// </summary>
    public int Stock
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "El stock debe ser mayor o igual a 0.");
            field = value;
        }
    }
}
