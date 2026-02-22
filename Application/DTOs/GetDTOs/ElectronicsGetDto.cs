namespace AsyncWarehouse.Application.DTOs.GetDTOs;

/// <summary>
/// DTO for retrieving an Electronics item.
/// </summary>
public class ElectronicsGetDto : InventoryItemGetDto
{
    /// <summary>
    /// Serial number of the electronic item.
    /// </summary>
    public required string SerialNumber { get; init; }

    /// <summary>
    /// Warranty period in months.
    /// </summary>
    public int WarrantyMonths { get; set; }

    /// <summary>
    /// Power consumption in watts.
    /// </summary>
    public float PowerWatts { get; set; }
}