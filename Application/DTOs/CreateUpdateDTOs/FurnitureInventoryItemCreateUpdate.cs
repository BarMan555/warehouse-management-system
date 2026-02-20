namespace AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;

public class FurnitureCreateUpdateDto : InventoryItemCreateUpdateDto
{
    public required string? Material { get; set; }
    public required float? Length { get; set; }
    public required float? Width { get; set; }
    public required float? Height { get; set; }
    public bool? RequiresAssembly { get; set; }
}