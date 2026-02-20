namespace AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;

public class ChemicalsCreateUpdateDto : InventoryItemCreateUpdateDto
{
    public required int? HazardClass { get; set; }
    public required DateTimeOffset? ExpirationDate { get; set; }
    public float? VolumeLiters { get; set; }
}