namespace AsyncWarehouse.Application.DTOs.GetDTOs;

/// <summary>
/// Data transfer object for retrieving pallet information, including its items and capacity.
/// </summary>
public class PalletGetDto
{
    /// <summary>
    /// Unique identifier for the pallet.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// List of inventory items currently on the pallet.
    /// </summary>
    public List<InventoryItemGetDto> Items { get; set; }

    /// <summary>
    /// Maximum weight capacity of the pallet in kilograms.
    /// </summary>
    public float MaxCapacity { get; private set; }

    /// <summary>
    /// Calculates the total weight of all inventory items currently on the pallet.
    /// </summary>
    public float TotalWeight => Items.Sum(x => x.Weight);
}