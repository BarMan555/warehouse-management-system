using WarehouseMS.Domain.Enums;

namespace WarehouseMS.Domain.Models;

/// <summary>
/// Represents a chemical item in the inventory.
/// </summary>
public class Chemicals : InventoryItem
{
    /// <summary>
    /// Hazard classification of the chemical item.
    /// </summary>
    public required HazardClass Hazard { get; init; }

    /// <summary>
    /// Expiration date of the chemical item.
    /// </summary>
    public required DateTimeOffset ExpirationDate { get; init; }

    /// <summary>
    /// Volume of the chemical item in liters.
    /// </summary>
    public float VolumeLiters { get; set; }
}