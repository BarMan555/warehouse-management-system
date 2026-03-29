namespace WarehouseMS.Application.DTOs.GetDTOs;

/// <summary>
/// DTO for retrieving a Furniture item.
/// </summary>
public class FurnitureGetDto : InventoryItemGetDto
{
    /// <summary>
    /// Material of the furniture item.
    /// </summary>
    public required string Material { get; set; }

    /// <summary>
    /// The length of the furniture in meters.
    /// </summary>
    public required float Length { get; set; }
    
    /// <summary>
    /// The width of the furniture in meters.
    /// </summary>
    public required float Width { get; set; }
    
    /// <summary>
    /// The height of the furniture in meters.
    /// </summary>
    public required float Height { get; set; }

    /// <summary>
    /// Indicates whether the furniture item requires assembly.
    /// </summary>
    public bool RequiresAssembly { get; set; }
}