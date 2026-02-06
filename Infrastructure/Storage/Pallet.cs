using AsyncWarehouse.Domain.Items;

namespace AsyncWarehouse.Infrastructure.Storage;

/// <summary>
/// Represents a pallet that can hold multiple inventory items, with a maximum weight capacity.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Pallet<T> where T : InventoryItem
{
    /// <summary>
    /// List of inventory items currently on the pallet.
    /// </summary>
    private readonly List<T> _items = new();

    /// <summary>
    /// Maximum weight capacity of the pallet in kilograms.
    /// </summary>
    public float MaxCapacity { get; }

    /// <summary>
    /// Initializes a new instance of the Pallet class with the specified maximum weight capacity.
    /// </summary>
    /// <param name="maxCapacity">Maximum weight capacity in kilograms.</param>
    /// <exception cref="ArgumentException">Thrown when maxCapacity is less than or equal to zero.</exception>
    public Pallet(float maxCapacity)
    {
        if(maxCapacity <= 0) throw new ArgumentException("Max capacity must be greater than zero.");
        MaxCapacity = maxCapacity;
    }

    /// <summary>
    /// Adds an inventory item to the pallet if it does not exceed the maximum weight capacity.
    /// </summary>
    /// <param name="item">The inventory item to add.</param>
    /// <exception cref="ArgumentException">Thrown when item weight is less than or equal to zero or exceeds pallet capacity.</exception>
    /// <returns>True if the item was added successfully, otherwise false.</returns>
    public bool AddItem(T item)
    {
        if(GetTotalWeight() + item.Weight > MaxCapacity || item.Weight <= 0) 
            throw new ArgumentException("Item weight must be greater than zero and must not exceed pallet capacity.");
        _items.Add(item);
        return true;
    }

    /// <summary>
    /// Retrieves the list of inventory items currently on the pallet.
    /// </summary>
    /// <returns>A list of inventory items.</returns>
    public List<T> GetItems() => _items;
    
    /// <summary>
    /// Retrieves an inventory item from the pallet by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the inventory item.</param>
    /// <returns>The inventory item if found, otherwise null.</returns>
    public T? GetItemById(Guid id) => _items.FirstOrDefault(x => x.Id == id);

    /// <summary>
    /// Retrieves the total number of inventory items currently on the pallet.
    /// </summary>
    /// <returns>The total number of inventory items.</returns>
    public int GetItemCount() => _items.Count;

    /// <summary>
    /// Calculates the total weight of all inventory items currently on the pallet.
    /// </summary>
    /// <returns>The total weight in kilograms.</returns>
    public float GetTotalWeight() => _items.Sum(x => x.Weight);

    /// <summary>
    /// Removes an inventory item from the pallet.
    /// </summary>
    /// <param name="item">The inventory item to remove.</param>
    /// <exception cref="ArgumentException">Thrown when the item is not found on the pallet.</exception>
    /// <returns>True if the item was removed successfully, otherwise false.</returns>
    public bool RemoveItem(T item)
    {
        if (!_items.Contains(item)) throw new ArgumentException("Item not found on pallet.");
        return _items.Remove(item);
    }
}