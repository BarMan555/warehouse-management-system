namespace AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;

public abstract class InventoryItemCreateUpdateDto
{
    public required string Type { get; set; } = "Electronics"; // "Electronics", "Furniture", "Chemicals"
    public required string Name { get; set; } = string.Empty;
    public required float Weight { get; set; }
    public required decimal Price { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}