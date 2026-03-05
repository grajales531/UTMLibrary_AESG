namespace UtmMarket.Core.Entities;

/// <summary>
/// Representa los estados posibles de una venta en el dominio.
/// </summary>
public enum SaleStatus : byte
{
    Pending = 1,
    Completed = 2,
    Cancelled = 3
}
