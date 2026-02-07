using AsyncWarehouse.Domain.Items;
using AsyncWarehouse.Infrastructure;
using AsyncWarehouse.Infrastructure.Storage;

namespace AsyncWarehouse.Application;

/// <summary>
/// Warehouse class responsible for processing incoming orders and managing deliveries
/// </summary>
/// <param name="deliveryServices"></param>
class Warehouse(IEnumerable<IDeliveryService> deliveryServices)
{
    /// <summary>
    /// List of available delivery services that the warehouse can use to dispatch orders
    /// </summary>
    private readonly IEnumerable<IDeliveryService> _deliveryServices = deliveryServices;

    /// <summary>
    /// List of active delivery tasks that are currently running in the warehouse
    /// </summary>
    private readonly List<Task> _deliveryTasks = new();

    /// <summary>
    /// Processes incoming orders asynchronously, managing pallets and dispatching deliveries
    /// </summary>
    /// <param name="ct">Cancellation token to stop processing orders</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task ProcessIncomingOrdersAsync(CancellationToken ct)
    {
        var generator = new RandomItemGenerator();
        var currentPallet = new Pallet<InventoryItem>(maxCapacity: 999);
        
        while(!ct.IsCancellationRequested)
        {
            await Task.Delay(1000, ct);
            var item = generator.GenerateRandomItem();

            Console.WriteLine($"Принят товар: {item.Name} ({item.Weight} кг)");

            try
            {
                currentPallet.AddItem(item);
            }
            catch(ArgumentException)
            {
                Console.WriteLine("Паллета заполнена! Ищем службу доставки...");

                var weight = currentPallet.GetTotalWeight();
                var service = _deliveryServices.FirstOrDefault(s => s.CanHandle(weight));

                if(service != null)
                {
                    var palletToDeliv = currentPallet;
                    currentPallet = new Pallet<InventoryItem>(999);

                    if(item.Weight < currentPallet.MaxCapacity)
                    {
                        currentPallet.AddItem(item);
                    }
                    else
                    {
                        Console.WriteLine($"Товар слишком тяжелый ({item.Weight}), не влезает даже в пустую паллету!");
                    }

                    var task = Task.Run( () => service.DeliverAsync(palletToDeliv, ct), ct);
                    _deliveryTasks.Add(task);

                    _deliveryTasks.RemoveAll(t => t.IsCompleted || t.IsCanceled);
                }
                else
                {
                    Console.WriteLine($"Нет службы доставки для паллеты весом {weight} кг! Товар останется на складе.");
                    currentPallet = new Pallet<InventoryItem>(999);
                    currentPallet.AddItem(item); // Все равно пытаемся сохранить текущий товар
                }
            }

        }

        await Task.WhenAll(_deliveryTasks);
    }
}