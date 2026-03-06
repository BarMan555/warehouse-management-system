namespace AsyncWarehouse.Domain.Models;

/// <summary>
/// Represents a pallet that can hold multiple inventory items, with a maximum weight capacity.
/// </summary>
public class Pallet
{
    /// <summary>
    /// Unique identifier for the pallet.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// List of inventory items currently on the pallet.
    /// </summary>
    private readonly List<InventoryItem> _items = new();
    
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyCollection<InventoryItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Maximum weight capacity of the pallet in kilograms.
    /// </summary>
    public float MaxCapacity { get; private set; }
    
    /// <summary>
    /// Constructor for EF Core
    /// </summary>
    protected Pallet(){}
    
    /// <summary>
    /// Initializes a new instance of the Pallet class with the specified maximum weight capacity.
    /// </summary>
    /// <param name="maxCapacity">Maximum weight capacity in kilograms.</param>
    /// <exception cref="ArgumentException">Thrown when maxCapacity is less than or equal to zero.</exception>
    public Pallet(float maxCapacity)
    {
        if(maxCapacity <= 0) throw new ArgumentException("Max capacity must be greater than zero.");
        Id = Guid.NewGuid();
        MaxCapacity = maxCapacity;
    }

    /// <summary>
    /// Adds an inventory item to the pallet if it does not exceed the maximum weight capacity.
    /// </summary>
    /// <param name="item">The inventory item to add.</param>
    /// <exception cref="ArgumentException">Thrown when item weight is less than or equal to zero or exceeds pallet capacity.</exception>
    /// <returns>True if the item was added successfully, otherwise false.</returns>
    public bool AddItem(InventoryItem item)
    {
        if(TotalWeight + item.Weight > MaxCapacity || item.Weight <= 0) 
            throw new ArgumentException("Item weight must be greater than zero and must not exceed pallet capacity.");
        _items.Add(item);
        return true;
    }

    /// <summary>
    /// Calculates the total weight of all inventory items currently on the pallet.
    /// </summary>
    public float TotalWeight => _items.Sum(x => x.Weight);

    /// <summary>
    /// Removes an inventory item from the pallet.
    /// </summary>
    /// <param name="item">The inventory item to remove.</param>
    /// <returns>True if the item was removed successfully, otherwise false.</returns>
    public bool RemoveItem(InventoryItem item)
    {
        return _items.Remove(item);
    }
}