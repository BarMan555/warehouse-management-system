using AsyncWarehouse.Domain.Items;
using AsyncWarehouse.Infrastructure.Storage;

namespace AsyncWarehouse.Application;

public class ShipDelivery : IDeliveryService
{
    public bool CanHandle(float weight) => 100.0f <= weight && weight < 1000.0f; 
    
    public async Task DeliverAsync(Pallet<InventoryItem> pallet, CancellationToken ct = default)
    {
        // Simulate ship delivery logic here
        Console.WriteLine($"Delivering pallet with {pallet.GetItemCount()} items via ship...");
        await Task.Delay(10000, ct); // Simulate delivery time
        Console.WriteLine("Delivery completed.");
    }
}