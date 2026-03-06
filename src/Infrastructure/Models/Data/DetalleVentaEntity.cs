namespace UtmMarket.Infrastructure.Models.Data;

/// <summary>
/// Modelo de datos parcial para la tabla [dbo].[DetalleVenta].
/// Compatible con Native AOT y optimizado para el mapeo manual.
/// </summary>
public partial class DetalleVentaEntity(int detalleId, int ventaId, int productoId)
{
    public int DetalleID { get; init; } = detalleId;
    public int VentaID { get; init; } = ventaId;
    public int ProductoID { get; init; } = productoId;

    /// <summary>
    /// Precio unitario capturado (DECIMAL 19,4).
    /// Validación vía C# 14 field keyword.
    /// </summary>
    public decimal PrecioUnitario
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "El precio unitario debe ser mayor o igual a 0.");
            field = value;
        }
    }

    /// <summary>
    /// Cantidad de productos (INT).
    /// Validación vía C# 14 field keyword (Debe ser mayor que 0).
    /// </summary>
    public int Cantidad
    {
        get => field;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "La cantidad debe ser mayor a 0.");
            field = value;
        }
    }

    /// <summary>
    /// Total de la línea (PrecioUnitario * Cantidad).
    /// Validación vía C# 14 field keyword.
    /// </summary>
    public decimal TotalDetalle
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "El total de detalle debe ser mayor o igual a 0.");
            field = value;
        }
    }
}
