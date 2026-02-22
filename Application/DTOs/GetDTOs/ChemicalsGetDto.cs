namespace AsyncWarehouse.Application.DTOs.GetDTOs;

/// <summary>
/// DTO for retrieving a Chemicals item.
/// </summary>
public class ChemicalsGetDto : InventoryItemGetDto
{
    /// <summary>
    /// Hazard classification of the chemical item.
    /// </summary>
    public required int HazardClass { get; init; }

    /// <summary>
    /// Expiration date of the chemical item.
    /// </summary>
    public required DateTimeOffset ExpirationDate { get; init; }

    /// <summary>
    /// Volume of the chemical item in liters.
    /// </summary>
    public float VolumeLiters { get; set; }
}