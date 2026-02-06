using AsyncWarehouse.Application;

var deliveryServices = new List<IDeliveryService>
{
    new DroneDelivery(),
    new TruckDelivery(),
    new ShipDelivery()
};

var warehouse = new Warehouse(deliveryServices);
var cts = new CancellationTokenSource();

await warehouse.ProcessIncomingOrdersAsync(cts.Token);