namespace AsyncWarehouse.Application.DTOs.Messages;

/// <summary>
/// Represents a message used to trigger the dispatch of a specific pallet via a designated delivery method.
/// </summary>
/// <param name="PalletId">The unique identifier of the pallet to be dispatched.</param>
/// <param name="DeliveryType">The type of delivery service to be used (e.g., "Truck", "Drone", "Ship").</param>
public record  DispatchPalletMessage(Guid PalletId, string DeliveryType);
