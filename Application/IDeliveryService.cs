using AsyncWarehouse.Domain.Items;
using AsyncWarehouse.Infrastructure.Storage;

namespace AsyncWarehouse.Application;

/// <summary>
/// Interface for delivery services, allowing different delivery methods (e.g., drone, truck, ship) to implement their own delivery logic.
/// </summary>
public interface IDeliveryService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    bool CanHandle(float weight);

    /// <summary>
    /// Delivers a pallet of inventory items using the specific delivery method implemented by the class.
    /// </summary>
    /// <param name="pallet">The pallet of inventory items to be delivered.</param>
    public Task DeliverAsync(Pallet<InventoryItem> pallet, CancellationToken ct = default);
}