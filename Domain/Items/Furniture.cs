namespace AsyncWarehouse.Models.Items;

/// <summary>
/// Dimensions of the furniture item.
/// </summary>
/// <param name="Length">Length of the furniture item in centimeters.</param>
/// <param name="Width">Width of the furniture item in centimeters.</param>
/// <param name="Height">Height of the furniture item in centimeters.</param>
public record Demension(float Length, float Width, float Height);

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
    public required Demension Dimensions { get; set; }

    /// <summary>
    /// Indicates whether the furniture item requires assembly.
    /// </summary>
    public bool RequiresAssembly { get; set; }
}