using AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;
using AsyncWarehouse.Application.DTOs.GetDTOs;
using AsyncWarehouse.Infrastructure;
using AsyncWarehouse.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsyncWarehouse.Application;

/// <summary>
/// Provides services for managing warehouse operations, including pallets and inventory items.
/// </summary>
public class WarehouseService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WarehouseService"/> class.
    /// </summary>
    /// <param name="context">The application's database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public WarehouseService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new pallet asynchronously.
    /// </summary>
    /// <param name="palletDto">The data transfer object containing pallet creation information.</param>
    /// <returns>A <see cref="PalletGetDto"/> representing the newly created pallet.</returns>
    public async Task<PalletGetDto> CreatePalletAsync(PalletCreateUpdateDto palletDto)
    {
        var pallet = _mapper.Map<Pallet>(palletDto);
        var newPallet = _mapper.Map<PalletGetDto>(await _context.Pallets.AddAsync(pallet));
        await _context.SaveChangesAsync();
        return newPallet;
    }

    /// <summary>
    /// Retrieves a pallet by its unique identifier asynchronously.
    /// </summary>
    /// <param name="palletId">The unique identifier of the pallet to retrieve.</param>
    /// <returns>A <see cref="PalletGetDto"/> if found, otherwise null.</returns>
    public async Task<PalletGetDto?> GetPalletAsync(Guid palletId)
    {
        var pallet = await _context.Pallets
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == palletId);
        if (pallet == null)  return null;
        return _mapper.Map<PalletGetDto>(pallet);
    }

    /// <summary>
    /// Retrieves a list of all pallets asynchronously.
    /// </summary>
    /// <returns>A list of <see cref="PalletGetDto"/> representing all pallets.</returns>
    public async Task<List<PalletGetDto>> GetAllPallets()
    {
        var pallets = await _context.Pallets
            .Include(p => p.Items)
            .ToListAsync();
        return _mapper.Map<List<PalletGetDto>>(pallets);        
    }

    /// <summary>
    /// Adds an inventory item to a specified pallet asynchronously.
    /// </summary>
    /// <param name="palletId">The unique identifier of the pallet to add the item to.</param>
    /// <param name="itemDto">The data transfer object containing inventory item creation information.</param>
    /// <returns>True if the item was added successfully, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown if the pallet with the specified ID is not found.</exception>
    public async Task<bool> AddItemToPalletAsync(Guid palletId, InventoryItemCreateUpdateDto itemDto)
    {
        var pallet = await _context.Pallets
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == palletId);
        
        if (pallet == null)
            throw new ArgumentException($"Pallet with id {palletId} not found");
        
        var item = _mapper.Map<InventoryItem>(itemDto);

        try
        {
            pallet.AddItem(item);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}