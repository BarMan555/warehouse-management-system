using AsyncWarehouse.Domain.Items;
using AsyncWarehouse.Infrastructure;
using AsyncWarehouse.Infrastructure.Storage;

namespace AsyncWarehouse.Application;

class Warehouse(IEnumerable<IDeliveryService> deliveryServices)
{
    private readonly IEnumerable<IDeliveryService> _deliveryServices = deliveryServices;
    private readonly List<Task> _deliveryTasks = new();

    public async Task ProcessIncomingOrdersAsync(CancellationToken ct)
    {
        var generator = new RandomItemGenerator();
        var currentPallet = new Pallet<InventoryItem>(maxCapacity: 4000);
        
        while(!ct.IsCancellationRequested)
        {
            await Task.Delay(1000, ct);
            var item = generator.GenerateRandomItem();

            Console.WriteLine($"Принят товар: {item.Name} ({item.Weight} кг)");

            try
            {
                currentPallet.AddItem(item);
            }
            catch(Exception)
            {
                Console.WriteLine("Паллета заполнена! Ищем службу доставки...");

                var weight = currentPallet.GetTotalWeight();
                var service = _deliveryServices.FirstOrDefault(s => s.CanHandle(weight));

                if(service != null)
                {
                    var palletToDeliv = currentPallet;
                    currentPallet = new Pallet<InventoryItem>(4000);

                    var task = Task.Run( () => service.DeliverAsync(palletToDeliv, ct), ct);
                    _deliveryTasks.Add(task);

                    _deliveryTasks.RemoveAll(t => t.IsCompleted || t.IsCanceled);
                }
            }

        }

        await Task.WhenAll(_deliveryTasks);
    }
}