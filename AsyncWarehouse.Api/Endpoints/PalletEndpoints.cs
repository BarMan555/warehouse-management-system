using AsyncWarehouse.Application;
using AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;
using AsyncWarehouse.Application.DTOs.Messages;
using AsyncWarehouse.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AsyncWarehouse.Api.Endpoints;

/// <summary>
/// Provides extension methods for mapping pallet-related endpoints.
/// </summary>
public static class PalletEndpoints
{
    /// <summary>
    /// Maps the pallet endpoints to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="app">The endpoint route builder to map the endpoints to.</param>
    public static void MapPalletEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/pallets").WithTags("Pallets");

        group.MapGet("/{id:guid}/", async (Guid id, WarehouseService service) =>
        {
            var pallet = await service.GetPalletAsync(id);
            return pallet != null
                ? Results.Ok(pallet)
                : Results.NotFound($"Pallet not found. ID: {id} is invalid.");
        });

        group.MapGet("/", async (WarehouseService service) =>
        {
            var pallets = await service.GetAllPallets();
            return pallets.Count != 0
                ? Results.Ok(pallets)
                : Results.NotFound("Zero pallets in warehouse");
        });
        
        group.MapPost("/", async ([FromBody] PalletCreateUpdateDto palletDto, WarehouseService service, ILogger<Program> logger) =>
        {
            try
            {
                var palletNew = await service.CreatePalletAsync(palletDto);
                logger.LogInformation($"Pallet created: {palletNew.Id}");
                return Results.Created($"/api/pallets/{palletNew.Id}", palletNew);
            }
            catch(ArgumentException ex)
            {
                logger.LogError($"Unable to create pallet: {palletDto.MaxCapacity} kg: {ex.Message}");
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPost("/{id:guid}/items/electronics", async (Guid id, [FromBody] ElectronicsCreateUpdateDto itemDto,
                WarehouseService service, ILogger<Program> logger) => 
        await AddItemToPallet(id, itemDto, service, logger));
        
        group.MapPost("/{id:guid}/items/furniture", async (Guid id, [FromBody] FurnitureCreateUpdateDto itemDto, 
                WarehouseService warehouseService, ILogger<Program> logger) =>
        await AddItemToPallet(id, itemDto, warehouseService, logger));

        group.MapPost("/{id:guid}/items/chemicals", async (Guid id, [FromBody] ChemicalsCreateUpdateDto itemDto, 
                WarehouseService warehouseService, ILogger<Program> logger) =>
        await AddItemToPallet(id, itemDto, warehouseService, logger));
        
        group.MapPost("/{id:guid}/dispatch", async (Guid id, [FromBody] string deliveryType, WarehouseService warehouseService, IMessageProducer producer) =>
        {
            var pallet = await warehouseService.GetPalletAsync(id);
            if (pallet == null) return Results.NotFound($"Not pallet found with id: {id}.");

            var message = new DispatchPalletMessage(id, deliveryType);
            await producer.SendMessage(message: message, routingKey: deliveryType);

            return Results.Accepted("Pallet dispatch initiated.");
        });
        
    }
    
    private static async Task<IResult> AddItemToPallet(Guid id, InventoryItemCreateUpdateDto item, WarehouseService service, ILogger<Program> logger)
    {
        var result = await service.AddItemToPalletAsync(id, item);

        if (result) logger.LogInformation($"Item has been added to Pallet: {id}");
        else logger.LogError($"Failed to add item {id} to pallet");
        
        return result 
            ? Results.Ok("Item has been successfully added to the pallet.") 
            : Results.BadRequest("Failed to add item to pallet");
    }
}