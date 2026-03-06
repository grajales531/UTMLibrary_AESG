namespace UtmMarket.Core.Entities;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Entidad de dominio pura que representa una venta.
/// </summary>
public sealed class Sale(int saleId, string folio)
{
    public int SaleID { get; init; } = saleId;
    public string Folio { get; init; } = folio ?? throw new ArgumentNullException(nameof(folio));
    
    /// <summary>
    /// Fecha de la venta, inicializada automáticamente al momento de creación.
    /// </summary>
    public DateTime SaleDate { get; init; } = DateTime.Now;

    /// <summary>
    /// Estatus actual de la venta.
    /// </summary>
    public SaleStatus Status { get; set; } = SaleStatus.Pending;

    /// <summary>
    /// Lista de detalles (líneas) que componen la venta.
    /// </summary>
    public List<SaleDetail> Details { get; init; } = [];

    /// <summary>
    /// Cantidad total de ítems (sumatoria de cantidades en los detalles).
    /// </summary>
    public int TotalItems => Details.Sum(d => d.Quantity);

    /// <summary>
    /// Monto total de la venta (sumatoria de subtotales de los detalles).
    /// </summary>
    public decimal TotalSale => Details.Sum(d => d.TotalDetail);
}
