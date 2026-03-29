using AutoMapper;
using WarehouseMS.Application.DTOs.CreateUpdateDTOs;
using WarehouseMS.Application.DTOs.GetDTOs;
using WarehouseMS.Application.Interfaces;
using WarehouseMS.Domain.Models;

namespace WarehouseMS.Application;

/// <summary>
/// Provides services for managing warehouse operations, including pallets and inventory items.
/// </summary>
public class WarehouseService
{
    private readonly IPalletRepository _palletRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WarehouseService"/> class.
    /// </summary>
    /// <param name="palletRepository">The repository of pallets</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public WarehouseService(
        IPalletRepository palletRepository, 
        IMapper mapper)
    {
        _palletRepository = palletRepository;
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
        var newPallet = _mapper.Map<PalletGetDto>(await _palletRepository.CreateAsync(pallet));
        await _palletRepository.SaveChangesAsync();
        return newPallet;
    }

    /// <summary>
    /// Retrieves a pallet by its unique identifier asynchronously.
    /// </summary>
    /// <param name="palletId">The unique identifier of the pallet to retrieve.</param>
    /// <returns>A <see cref="PalletGetDto"/> if found, otherwise null.</returns>
    public async Task<PalletGetDto?> GetPalletAsync(Guid palletId)
    {
        var pallet = await _palletRepository.GetByIdAsync(palletId);
        
        if (pallet == null)  
            throw new ArgumentException($"Pallet with id {palletId} not found");
        
        return _mapper.Map<PalletGetDto>(pallet);
    }

    /// <summary>
    /// Retrieves a list of all pallets asynchronously.
    /// </summary>
    /// <returns>A list of <see cref="PalletGetDto"/> representing all pallets.</returns>
    public async Task<List<PalletGetDto>> GetAllPallets()
    {
        var pallets = await _palletRepository.GetAllAsync();
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
        var pallet = await _palletRepository.GetByIdAsync(palletId);
        
        if (pallet == null)
            throw new ArgumentException($"Pallet with id {palletId} not found");
        
        var item = _mapper.Map<InventoryItem>(itemDto);

        try
        {
            pallet.AddItem(item);
            await _palletRepository.SaveChangesAsync();
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}