using WarehouseMS.Domain.Models;

namespace WarehouseMS.Application.Interfaces;

/// <summary>
/// Defines the contract for pallet data operations.
/// </summary>
public interface IPalletRepository
{
    /// <summary>
    /// Creates a new pallet in the system.
    /// </summary>
    /// <param name="pallet">The pallet entity to create.</param>
    /// <returns>The created pallet entity.</returns>
    Task<Pallet> CreateAsync(Pallet pallet);

    /// <summary>
    /// Retrieves a pallet by its unique identifier, including its inventory items.
    /// </summary>
    /// <param name="palletId">The unique identifier of the pallet.</param>
    /// <returns>The pallet if found; otherwise, null.</returns>
    Task<Pallet?> GetByIdAsync(Guid palletId);

    /// <summary>
    /// Retrieves all pallets in the system, including their inventory items.
    /// </summary>
    /// <returns>A list of all pallets.</returns>
    Task<List<Pallet>> GetAllAsync();

    /// <summary>
    /// Persists all pending changes to the data store.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveChangesAsync();
}
