namespace AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;

/// <summary>
/// DTO for creating or updating an Electronics item.
/// </summary>
public class ElectronicsCreateUpdateDto : InventoryItemCreateUpdateDto
{
    /// <summary>
    /// The serial number of the electronic device.
    /// </summary>
    public required string? SerialNumber { get; set; }
    
    /// <summary>
    /// The warranty period in months.
    /// </summary>
    public int? WarrantyMonths { get; set; }
    
    /// <summary>
    /// The power consumption in watts.
    /// </summary>
    public float? PowerWatts { get; set; }
}