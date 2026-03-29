using WarehouseMS.Application.Interfaces;
using WarehouseMS.Domain.Models;

namespace AsyncWarehouse.Infrastructure;

/// <summary>
/// Delivery service implementation for drone deliveries
/// </summary>
public class DroneDelivery : IDeliveryService
{
    /// <inheritdoc cref="IDeliveryService.CanHandle"/>
    public bool CanHandle(float weight) => weight < 5.0f;

    /// <inheritdoc cref="IDeliveryService.DeliverAsync"/>
    public async Task DeliverAsync(Pallet pallet, CancellationToken ct = default)
    {
        // Simulate drone delivery logic here
        Console.WriteLine($"Delivering pallet with {pallet.Items.Count} items via drone...");
        await Task.Delay(2000, ct); // Simulate delivery time
        Console.WriteLine("Delivery completed.");
    }
}