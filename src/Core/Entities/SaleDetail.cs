namespace UtmMarket.Core.Entities;

/// <summary>
/// Representa el detalle de una línea de venta. 
/// Captura el precio histórico del producto en el momento de la venta.
/// </summary>
public sealed class SaleDetail(Product product, int quantity)
{
    public Product Product { get; init; } = product ?? throw new ArgumentNullException(nameof(product));
    
    /// <summary>
    /// Precio unitario capturado al momento de la venta para preservar el registro histórico.
    /// </summary>
    public decimal UnitPrice { get; init; } = product.Price;

    /// <summary>
    /// Cantidad de productos en esta línea.
    /// </summary>
    public int Quantity { get; init; } = quantity > 0 ? quantity : throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));

    /// <summary>
    /// Total calculado para este detalle (Precio * Cantidad).
    /// </summary>
    public decimal TotalDetail => UnitPrice * Quantity;
}
