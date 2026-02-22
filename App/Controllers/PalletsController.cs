using AsyncWarehouse.Application;
using AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;
using AsyncWarehouse.Application.DTOs.GetDTOs;
using AsyncWarehouse.Domain.Enums;
using AsyncWarehouse.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AsyncWarehouse.App.Controllers;


/// <summary>
/// Controller for managing pallets in the warehouse.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PalletsController : ControllerBase
{
    private readonly WarehouseService _warehouseService;
    private readonly ILogger<PalletsController> _logger;
    private readonly IMapper _mapper;

    public PalletsController(WarehouseService warehouseService, 
        IServiceProvider serviceProvider, 
        ILogger<PalletsController> logger,
        IMapper mapper)
    {
        _warehouseService = warehouseService;
        _logger = logger;
        _mapper = mapper;
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
        var pallet = _mapper.Map<PalletGetDto>(await _warehouseService.GetPalletAsync(id));
        if (pallet == null) return NotFound($"Pallet not found. ID: {id} is invalid.");

        return pallet;
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
        var pallets = _mapper.Map<List<PalletGetDto>>(await _warehouseService.GetAllPallets());
        return pallets.Count != 0 ? pallets : NotFound("Zero pallets in warehouse");
    }

    /// <summary>
    /// Creates a new pallet with a specified capacity.
    /// </summary>
    /// <param name="capacity">The maximum weight capacity of the pallet in kilograms.</param>
    /// <returns>The GUID of the newly created pallet.</returns>
    /// <response code="200">Returns the GUID of the created pallet.</response>
    /// <response code="400">If the capacity is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePallet([FromQuery] float capacity)
    {
        try
        {
            var palletId = await _warehouseService.CreatePalletAsync(capacity);
            _logger.LogInformation($"Pallet has been successfully created: {palletId} - {capacity} kg");
            return palletId;
        }
        catch (ArgumentException e)
        {
            _logger.LogError($"Unable to create pallet: {capacity}");
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Adds an electronics item to a specific pallet.
    /// </summary>
    /// <param name="id">The GUID of the pallet.</param>
    /// <param name="item">The electronics item to add.</param>
    /// <returns>A confirmation message.</returns>
    /// <response code="200">If the item was successfully added.</response>
    /// <response code="400">If the item could not be added to the pallet.</response>
    [HttpPost]
    [Route("{id}/items/electronics")]
    public async Task<IActionResult> AddElectronicsToPallet(Guid id, [FromBody] ElectronicsCreateUpdateDto item)
    {
        var newItem = _mapper.Map<Electronics>(item);
        return await AddItemToPallet(id, newItem);
    }
    
    /// <summary>
    /// Adds a furniture item to a specific pallet.
    /// </summary>
    /// <param name="id">The GUID of the pallet.</param>
    /// <param name="item">The furniture item to add.</param>
    /// <returns>A confirmation message.</returns>
    /// <response code="200">If the item was successfully added.</response>
    /// <response code="400">If the item could not be added to the pallet.</response>
    [HttpPost]
    [Route("{id}/items/furniture")]
    public async Task<IActionResult> AddFurnitureToPallet(Guid id, [FromBody] FurnitureCreateUpdateDto item)
    {
        var newItem = _mapper.Map<Furniture>(item);
        return await AddItemToPallet(id, newItem);
    }
    
    /// <summary>
    /// Adds a chemicals item to a specific pallet.
    /// </summary>
    /// <param name="id">The GUID of the pallet.</param>
    /// <param name="item">The chemicals item to add.</param>
    /// <returns>A confirmation message.</returns>
    /// <response code="200">If the item was successfully added.</response>
    /// <response code="400">If the item could not be added to the pallet.</response>
    [HttpPost]
    [Route("{id}/items/chemicals")]
    public async Task<IActionResult> AddChemicalsToPallet(Guid id, [FromBody] ChemicalsCreateUpdateDto item)
    {
        var newItem = _mapper.Map<Chemicals>(item);
        return  await AddItemToPallet(id, newItem);
    }

    private async Task<IActionResult> AddItemToPallet(Guid id, InventoryItem item)
    {
        var result = await _warehouseService.AddItemToPalletAsync(id, item);

        if (result) _logger.LogInformation($"Item has been added to Pallet: {id}");
        else _logger.LogError($"Failed to add item {id} to pallet");
        
        return result ? Ok("Item has been successfully added to the pallet.") : BadRequest("Failed to add item to pallet"); 
    }
}