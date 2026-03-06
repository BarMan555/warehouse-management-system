using AsyncWarehouse.Application.Interfaces;
using AsyncWarehouse.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncWarehouse.Infrastructure.Repositories;

public class PalletRepository : IPalletRepository
{
    private readonly ApplicationContext _context;
    
    public PalletRepository(ApplicationContext context) => _context = context;

    public async Task<Pallet> CreateAsync(Pallet pallet)
    {
        await _context.Pallets.AddAsync(pallet);
        return pallet;
    }

    public async Task<Pallet?> GetByIdAsync(Guid palletId)
    {
        return await _context.Pallets
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == palletId);
    }

    public async Task<List<Pallet>> GetAllAsync()
    {
        return await _context.Pallets
            .Include(p => p.Items)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}