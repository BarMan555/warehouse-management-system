using AsyncWarehouse.Domain.Models;

namespace AsyncWarehouse.Application.Interfaces;

public interface IPalletRepository
{
    Task<Pallet> CreateAsync(Pallet pallet);
    Task<Pallet?> GetByIdAsync(Guid palletId);
    Task<List<Pallet>> GetAllAsync();
    Task SaveChangesAsync();
}