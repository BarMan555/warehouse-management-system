using Microsoft.EntityFrameworkCore;
using WarehouseMS.Application.Interfaces;
using WarehouseMS.Domain.Models;

namespace WarehouseMS.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core implementation of the pallet repository.
/// </summary>
public class PalletRepository : IPalletRepository
{
    private readonly ApplicationContext _context;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PalletRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PalletRepository(ApplicationContext context) => _context = context;

    /// <inheritdoc />
    public async Task<Pallet> CreateAsync(Pallet pallet)
    {
        await _context.Pallets.AddAsync(pallet);
        return pallet;
    }

    /// <inheritdoc />
    public async Task<Pallet?> GetByIdAsync(Guid palletId)
    {
        return await _context.Pallets
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == palletId);
    }

    /// <inheritdoc />
    public async Task<List<Pallet>> GetAllAsync()
    {
        return await _context.Pallets
            .Include(p => p.Items)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
