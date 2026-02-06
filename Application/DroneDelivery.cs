using AsyncWarehouse.Domain.Items;
using AsyncWarehouse.Infrastructure.Storage;

namespace AsyncWarehouse.Application;

/// <summary>
/// Delivery service implementation for drone deliveries, simulating the delivery process with a delay to represent the time taken for the drone to deliver the items.
/// </summary>
public class DroneDelivery : IDeliveryService
{
    public bool CanHandle(float weight) => weight < 5.0f; 

    public async Task DeliverAsync(Pallet<InventoryItem> pallet, CancellationToken ct = default)
    {
        // Simulate drone delivery logic here
        Console.WriteLine($"Delivering pallet with {pallet.GetItemCount()} items via drone...");
        await Task.Delay(2000, ct); // Simulate delivery time
        Console.WriteLine("Delivery completed.");
    }
}