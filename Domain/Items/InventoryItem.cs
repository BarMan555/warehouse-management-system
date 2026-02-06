namespace AsyncWarehouse.Domain.Items;

/// <summary>
/// Base class for all inventory items in the warehouse.
/// </summary>
public abstract class InventoryItem
{
    /// <summary>
    /// Unique identifier for the inventory item.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Name of the inventory item.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Weight of the inventory item in kilograms.
    /// </summary>
    public required float Weight { get; set; }

    /// <summary>
    /// Price of the inventory item in USD.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// Description of the inventory item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date and time when the inventory item was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}