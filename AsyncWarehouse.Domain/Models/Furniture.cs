using AsyncWarehouse.Domain.Enums;

namespace AsyncWarehouse.Domain.Models;

/// <summary>
/// Represents a furniture item in the inventory.
/// </summary>
public class Furniture : InventoryItem
{
    /// <summary>
    /// Material of the furniture item.
    /// </summary>
    public required string Material { get; set; }

    /// <summary>
    /// Dimensions of the furniture item.
    /// </summary>
    public required Dimension Dimensions { get; set; }

    /// <summary>
    /// Indicates whether the furniture item requires assembly.
    /// </summary>
    public bool RequiresAssembly { get; set; }
}