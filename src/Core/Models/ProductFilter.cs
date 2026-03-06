namespace UtmMarket.Core.Models;

/// <summary>
/// Criterios de búsqueda para productos. 
/// Utiliza un record de C# para inmutabilidad y comparación estructural.
/// </summary>
public sealed record ProductFilter(
    string? Name = null,
    string? SKU = null,
    string? Brand = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int? MinStock = null
);
