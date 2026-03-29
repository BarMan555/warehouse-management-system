namespace WarehouseMS.Application.DTOs.CreateUpdateDTOs;

/// <summary>
/// Abstract base DTO for creating or updating an inventory item.
/// </summary>
public abstract class InventoryItemCreateUpdateDto
{
    /// <summary>
    /// The type of the inventory item (e.g., "Electronics", "Furniture", "Chemicals").
    /// </summary>
    public required string Type { get; set; } = "Electronics"; // "Electronics", "Furniture", "Chemicals"
    
    /// <summary>
    /// The name of the inventory item.
    /// </summary>
    public required string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// The weight of the inventory item in kilograms.
    /// </summary>
    public required float Weight { get; set; }
    
    /// <summary>
    /// The price of the inventory item.
    /// </summary>
    public required decimal Price { get; set; }
    
    /// <summary>
    /// The description of the inventory item.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// The date and time when the inventory item was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}