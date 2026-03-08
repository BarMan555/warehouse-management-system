namespace AsyncWarehouse.Application.DTOs.GetDTOs;

/// <summary>
/// Abstract base DTO for retrieving an inventory item.
/// </summary>
public abstract class InventoryItemGetDto
{
    /// <summary>
    /// Unique identifier for the inventory item.
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    ///  Unique identifier of Pallet where is this item.
    /// </summary>
    public Guid PalletId { get; set; }

    /// <summary>
    /// Name of the inventory item.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Weight of the inventory item in kilograms.
    /// </summary>
    public required float Weight { get; init; }

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