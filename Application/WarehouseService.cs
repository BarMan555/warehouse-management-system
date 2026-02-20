using AsyncWarehouse.Infrastructure;
using AsyncWarehouse.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncWarehouse.Application;

/// <summary>
///
/// </summary>
public class WarehouseService
{
    private readonly ApplicationContext _context;

    public WarehouseService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreatePalletAsync(float capacity)
    {
        var pallet = new Pallet(capacity);
        _context.Pallets.Add(pallet);
        await _context.SaveChangesAsync();
        return pallet.Id;
    }

    public async Task<Pallet?> GetPalletAsync(Guid palletId)
    {
        return await _context.Pallets
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == palletId);
    }

    public async Task<List<Pallet>> GetAllPallets()
    {
        return await _context.Pallets
            .Include(p => p.Items)
            .ToListAsync();
    }

    public async Task<bool> AddItemToPalletAsync(Guid palletId, InventoryItem item)
    {
        var pallet = await GetPalletAsync(palletId);
        if (pallet is null)
            throw new NullReferenceException($"Pallet with id {palletId} not found");

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