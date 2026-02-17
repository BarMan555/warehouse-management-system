namespace AsyncWarehouse.Domain.Models;

/// <summary>
/// Represents an electronic item in the inventory.
/// </summary>
public class Electronics : InventoryItem
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