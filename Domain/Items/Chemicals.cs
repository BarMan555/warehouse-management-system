namespace AsyncWarehouse.Domain.Items;

/// <summary>
/// Hazard classification for chemical items.
/// </summary>
public enum HazardClass 
{ 
    /// <summary>
    /// No hazard.
    /// </summary>
    None, 

    /// <summary>
    /// Low hazard.
    /// </summary>
    Low, 

    /// <summary>
    /// Moderate hazard.
    /// </summary>
    Moderate, 

    /// <summary>
    /// High hazard.
    /// </summary>
    High 
}

/// <summary>
/// Represents a chemical item in the inventory.
/// </summary>
public class Chemicals : InventoryItem
{
    /// <summary>
    /// Hazard classification of the chemical item.
    /// </summary>
    public required HazardClass Hazard { get; init; }

    /// <summary>
    /// Expiration date of the chemical item.
    /// </summary>
    public required DateTimeOffset ExpirationDate { get; init; }

    /// <summary>
    /// Volume of the chemical item in liters.
    /// </summary>
    public float VolumeLiters { get; set; }
}