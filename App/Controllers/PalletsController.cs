using AsyncWarehouse.Application;
using AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;
using AsyncWarehouse.Domain.Enums;
using AsyncWarehouse.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AsyncWarehouse.App.Controllers;


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

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Pallet>> GetPallet(Guid id)
    {
        var pallet = await _warehouseService.GetPalletAsync(id);
        if (pallet == null) return NotFound($"Pallet not found. ID: {id} is invalid.");

        return pallet;
    }

    [HttpGet]
    public async Task<ActionResult<List<Pallet>>> GetAllPallets()
    {
        var pallets = await _warehouseService.GetAllPallets();
        return pallets.Count != 0 ? pallets : NotFound("Zero pallets in warehouse");
    }

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

    [HttpPost]
    [Route("{id}/items/electronics")]
    public async Task<IActionResult> AddElectronicsToPallet(Guid id, [FromBody] ElectronicsCreateUpdateDto item)
    {
        var newItem = _mapper.Map<Electronics>(item);
        return await AddItemToPallet(id, newItem);
    }
    
    [HttpPost]
    [Route("{id}/items/furniture")]
    public async Task<IActionResult> AddFurnitureToPallet(Guid id, [FromBody] FurnitureCreateUpdateDto item)
    {
        var newItem = _mapper.Map<Furniture>(item);
        return await AddItemToPallet(id, newItem);
    }
    
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