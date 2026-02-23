using AsyncWarehouse.Application.DTOs.CreateUpdateDTOs;
using AsyncWarehouse.Application.DTOs.GetDTOs;
using AsyncWarehouse.Infrastructure;
using AsyncWarehouse.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsyncWarehouse.Application;

/// <summary>
///
/// </summary>
public class WarehouseService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public WarehouseService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PalletGetDto> CreatePalletAsync(PalletCreateUpdateDto palletDto)
    {
        var pallet = _mapper.Map<Pallet>(palletDto);
        var newPallet = _mapper.Map<PalletGetDto>(await _context.Pallets.AddAsync(pallet));
        await _context.SaveChangesAsync();
        return newPallet;
    }

    public async Task<PalletGetDto?> GetPalletAsync(Guid palletId)
    {
        var pallet = await _context.Pallets
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == palletId);
        if (pallet == null)  return null;
        return _mapper.Map<PalletGetDto>(pallet);
    }

    public async Task<List<PalletGetDto>> GetAllPallets()
    {
        var pallets = await _context.Pallets
            .Include(p => p.Items)
            .ToListAsync();
        return _mapper.Map<List<PalletGetDto>>(pallets);        
    }

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