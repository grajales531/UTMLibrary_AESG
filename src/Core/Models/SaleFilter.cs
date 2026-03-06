namespace UtmMarket.Core.Models;

using System;
using UtmMarket.Core.Entities;

/// <summary>
/// Criteria for filtering sales within the system.
/// Uses C# 14 primary constructors and the 'field' keyword for validation logic.
/// </summary>
public sealed record SaleFilter(
    string? Folio = null,
    SaleStatus? Status = null)
{
    /// <summary>
    /// Starting date for the search range.
    /// </summary>
    public DateTime? StartDate
    {
        get => field;
        init
        {
            if (value > DateTime.Now) throw new ArgumentOutOfRangeException(nameof(value), "Start date cannot be in the future.");
            field = value;
        }
    }

    /// <summary>
    /// Ending date for the search range.
    /// </summary>
    public DateTime? EndDate
    {
        get => field;
        init
        {
            if (value < StartDate) throw new ArgumentOutOfRangeException(nameof(value), "End date cannot be earlier than start date.");
            field = value;
        }
    }

    /// <summary>
    /// Minimum total amount of the sale.
    /// </summary>
    public decimal? MinTotal
    {
        get => field;
        init
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Minimum total cannot be negative.");
            field = value;
        }
    }
}
