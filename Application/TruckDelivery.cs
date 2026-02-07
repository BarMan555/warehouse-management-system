using AsyncWarehouse.Domain.Items;
using AsyncWarehouse.Infrastructure.Storage;

namespace AsyncWarehouse.Application;

/// <summary>
/// Delivery service implementation for truck deliveries
/// </summary>
public class TruckDelivery : IDeliveryService
{
    /// <inheritdoc cref="IDeliveryService.CanHandle"/>
    public bool CanHandle(float weight) => 5.0f <= weight && weight < 100.0f; 

    /// <inheritdoc cref="IDeliveryService.DeliverAsync"/>
    public async Task DeliverAsync(Pallet<InventoryItem> pallet, CancellationToken ct = default)
    {
        // Simulate truck delivery logic here
        Console.WriteLine($"Delivering pallet with {pallet.GetItemCount()} items via truck...");
        await Task.Delay(5000, ct); // Simulate delivery time
        Console.WriteLine("Delivery completed.");
    }
}