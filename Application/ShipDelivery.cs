using AsyncWarehouse.Domain.Models;

namespace AsyncWarehouse.Application;

/// <summary>
/// Delivery service implementation for ship deliveries
/// </summary>
public class ShipDelivery : IDeliveryService
{
    /// <inheritdoc cref="IDeliveryService.CanHandle"/>
    public bool CanHandle(float weight) => 100.0f <= weight && weight < 1000.0f; 
    
    /// <inheritdoc cref="IDeliveryService.DeliverAsync"/>
    public async Task DeliverAsync(Pallet pallet, CancellationToken ct = default)
    {
        // Simulate ship delivery logic here
        Console.WriteLine($"Delivering pallet with {pallet.Items.Count} items via ship...");
        await Task.Delay(10000, ct); // Simulate delivery time
        Console.WriteLine("Delivery completed.");
    }
}