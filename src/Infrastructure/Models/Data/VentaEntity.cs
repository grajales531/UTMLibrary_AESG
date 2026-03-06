namespace UtmMarket.Infrastructure.Models.Data;

using System;

/// <summary>
/// Modelo de datos parcial para la tabla [dbo].[Venta].
/// Compatible con Native AOT y optimizado para el mapeo manual.
/// </summary>
public partial class VentaEntity(int ventaId, string folio)
{
    public int VentaID { get; init; } = ventaId;
    public string Folio { get; init; } = folio;
    public DateTime FechaVenta { get; set; } = DateTime.Now;

    /// <summary>
    /// Cantidad total de artículos en la venta (INT).
    /// Validación vía C# 14 field keyword.
    /// </summary>
    public int TotalArticulos
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "TotalArticulos debe ser mayor o igual a 0.");
            field = value;
        }
    }

    /// <summary>
    /// Monto total de la venta (DECIMAL 19,4).
    /// Validación vía C# 14 field keyword.
    /// </summary>
    public decimal TotalVenta
    {
        get => field;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "TotalVenta debe ser mayor o igual a 0.");
            field = value;
        }
    }

    /// <summary>
    /// Estatus de la venta (TINYINT).
    /// Validación vía C# 14 field keyword (Valores: 1, 2, 3).
    /// </summary>
    public byte Estatus
    {
        get => field;
        set
        {
            if (value < 1 || value > 3) throw new ArgumentOutOfRangeException(nameof(value), "El estatus debe estar entre 1 y 3.");
            field = value;
        }
    }
}
