namespace WarehouseMS.Application.DTOs.CreateUpdateDTOs;

/// <summary>
/// DTO for creating or updating a Furniture item.
/// </summary>
public class FurnitureCreateUpdateDto : InventoryItemCreateUpdateDto
{
    /// <summary>
    /// The material of the furniture.
    /// </summary>
    public required string? Material { get; set; }
    
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
    /// Indicates whether the furniture requires assembly.
    /// </summary>
    public bool? RequiresAssembly { get; set; }
}