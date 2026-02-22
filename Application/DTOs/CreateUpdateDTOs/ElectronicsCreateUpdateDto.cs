namespace AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;

public class ElectronicsCreateUpdateDto : InventoryItemCreateUpdateDto
{
    public required string? SerialNumber { get; set; }
    public int? WarrantyMonths { get; set; }
    public float? PowerWatts { get; set; }
}