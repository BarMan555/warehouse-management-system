using AsyncWarehouse.Domain.Models;

namespace AsyncWarehouse.Application;

/// <summary>
/// Interface for delivery services, allowing different delivery methods (e.g., drone, truck, ship) to implement their own delivery logic.
/// </summary>
public interface IDeliveryService
{
    /// <summary>
    /// Checks if the delivery service can handle a pallet of a given weight.
    /// </summary>
    /// <param name="weight">The weight of the pallet to be delivered.</param>
    /// <returns>True if the service can handle the weight, otherwise false.</returns>
    bool CanHandle(float weight);

    /// <summary>
    /// Delivers a pallet of inventory items using the specific delivery method implemented by the class.
    /// </summary>
    /// <param name="pallet">The pallet of inventory items to be delivered.</param>
    /// <param name="ct">Cancellation token to cancel the delivery operation if needed.</param>
    /// <returns>A task representing the asynchronous delivery operation.</returns>
    public Task DeliverAsync(Pallet pallet, CancellationToken ct = default);
}