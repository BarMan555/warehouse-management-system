namespace AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;

/// <summary>
/// DTO for creating or updating a Chemicals item.
/// </summary>
public class ChemicalsCreateUpdateDto : InventoryItemCreateUpdateDto
{
    /// <summary>
    /// The hazard classification of the chemical.
    /// </summary>
    public required int? HazardClass { get; set; }
    
    /// <summary>
    /// The expiration date of the chemical.
    /// </summary>
    public required DateTimeOffset? ExpirationDate { get; set; }
    
    /// <summary>
    /// The volume of the chemical in liters.
    /// </summary>
    public float? VolumeLiters { get; set; }
}