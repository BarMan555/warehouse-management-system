using AsyncWarehouse.Application;
using AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;
using AsyncWarehouse.Application.DTOs.GetDTOs;
using AsyncWarehouse.Application.DTOs.Messages;
using AsyncWarehouse.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AsyncWarehouse.Api.Controllers;


/// <summary>
/// Controller for managing pallets in the warehouse.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PalletsController : ControllerBase
{
    private readonly WarehouseService _warehouseService;
    private readonly IMessageProducer _producer;
    private readonly ILogger<PalletsController> _logger;

    public PalletsController(
        WarehouseService warehouseService, 
        IMessageProducer producer, 
        ILogger<PalletsController> logger
        )
    {
        _warehouseService = warehouseService;
        _producer = producer;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a specific pallet by its unique identifier.
    /// </summary>
    /// <param name="id">The GUID of the pallet to retrieve.</param>
    /// <returns>The found pallet.</returns>
    /// <response code="200">Returns the requested pallet.</response>
    /// <response code="404">If the pallet with the specified ID is not found.</response>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<PalletGetDto>> GetPallet(Guid id)
    {
        var pallet = await _warehouseService.GetPalletAsync(id);
        if (pallet == null) return NotFound($"Pallet not found. ID: {id} is invalid.");

        return Ok(pallet);
    }

    /// <summary>
    /// Retrieves a list of all pallets in the warehouse.
    /// </summary>
    /// <returns>A list of pallets.</returns>
    /// <response code="200">Returns the list of pallets.</response>
    /// <response code="404">If there are no pallets in the warehouse.</response>
    [HttpGet]
    public async Task<ActionResult<List<PalletGetDto>>> GetAllPallets()
    {
        var pallets = await _warehouseService.GetAllPallets();
        return pallets.Count != 0 ? Ok(pallets) : NotFound("Zero pallets in warehouse");
    }

    /// <summary>
    /// Creates a new pallet with a specified capacity.
    /// </summary>
    /// <param name="palletDto">The create-update dto for creating new pallet</param>
    /// <returns>The GUID of the newly created pallet.</returns>
    /// <response code="201">Returns the created pallet.</response>
    /// <response code="400">If the capacity is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<PalletGetDto>> CreatePallet([FromBody] PalletCreateUpdateDto palletDto)
    {
        try
        {
            var palletNew = await _warehouseService.CreatePalletAsync(palletDto);
            _logger.LogInformation($"Pallet has been successfully created: {palletNew.Id} - {palletNew.MaxCapacity} kg");
            return CreatedAtAction(nameof(CreatePallet), palletNew);
        }
        catch (ArgumentException e)
        {
            _logger.LogError($"Unable to create pallet: {palletDto.MaxCapacity} kg: {e.Message}");
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Adds an electronics item to a specific pallet.
    /// </summary>
    /// <param name="id">The GUID of the pallet.</param>
    /// <param name="itemDto">The electronics item to add.</param>
    /// <returns>A confirmation message.</returns>
    /// <response code="200">If the item was successfully added.</response>
    /// <response code="400">If the item could not be added to the pallet.</response>
    [HttpPost]
    [Route("{id}/items/electronics")]
    public async Task<IActionResult> AddElectronicsToPallet(Guid id, [FromBody] ElectronicsCreateUpdateDto itemDto)
    {
        return await AddItemToPallet(id, itemDto);
    }
    
    /// <summary>
    /// Adds a furniture item to a specific pallet.
    /// </summary>
    /// <param name="id">The GUID of the pallet.</param>
    /// <param name="itemDto">The furniture item to add.</param>
    /// <returns>A confirmation message.</returns>
    /// <response code="200">If the item was successfully added.</response>
    /// <response code="400">If the item could not be added to the pallet.</response>
    [HttpPost]
    [Route("{id}/items/furniture")]
    public async Task<IActionResult> AddFurnitureToPallet(Guid id, [FromBody] FurnitureCreateUpdateDto itemDto)
    {
        return await AddItemToPallet(id, itemDto);
    }
    
    /// <summary>
    /// Adds a chemicals item to a specific pallet.
    /// </summary>
    /// <param name="id">The GUID of the pallet.</param>
    /// <param name="itemDto">The chemicals item to add.</param>
    /// <returns>A confirmation message.</returns>
    /// <response code="200">If the item was successfully added.</response>
    /// <response code="400">If the item could not be added to the pallet.</response>
    [HttpPost]
    [Route("{id}/items/chemicals")]
    public async Task<IActionResult> AddChemicalsToPallet(Guid id, [FromBody] ChemicalsCreateUpdateDto itemDto)
    {
        return  await AddItemToPallet(id, itemDto);
    }

    private async Task<IActionResult> AddItemToPallet(Guid id, InventoryItemCreateUpdateDto item)
    {
        var result = await _warehouseService.AddItemToPalletAsync(id, item);

        if (result) _logger.LogInformation($"Item has been added to Pallet: {id}");
        else _logger.LogError($"Failed to add item {id} to pallet");
        
        return result ? Ok("Item has been successfully added to the pallet.") : BadRequest("Failed to add item to pallet"); 
    }

    /// <summary>
    /// Initiates the dispatch of a pallet using a specified delivery type.
    /// </summary>
    /// <param name="id">The GUID of the pallet to dispatch.</param>
    /// <param name="deliveryType">The type of delivery (e.g., "drone", "truck", "ship").</param>
    /// <returns>An accepted status if the dispatch was successfully initiated.</returns>
    /// <response code="202">If the dispatch request has been accepted.</response>
    /// <response code="404">If the pallet with the specified ID is not found.</response>
    [HttpPost]
    [Route("{id}/dispatch")]
    public async Task<IActionResult> DispatchPallet(Guid id, [FromBody] string deliveryType)
    {
        var pallet = await _warehouseService.GetPalletAsync(id);
        if (pallet == null) return NotFound($"Not pallet found with id: {id}.");

        var message = new DispatchPalletMessage(id, deliveryType);

        await _producer.SendMessage(message: message, routingKey: deliveryType);

        return Accepted($"Pallet dispatch initiated.");
    }
}