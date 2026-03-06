using AsyncWarehouse.Domain.Models;
using AsyncWarehouse.Domain.Enums;

namespace AsyncWarehouse.Infrastructure;

/// <summary>
/// Implementation of IItemGenerator that generates random inventory items.
/// </summary>
public static class RandomItemGenerator
{
    /// <summary>
    /// Generates a random inventory item.
    /// </summary>
    /// <returns>A randomly generated inventory item.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid item type is generated.</exception>
    public static InventoryItem GenerateRandomItem()
    {
        var random = new Random();
        int itemType = random.Next(3); 

        return itemType switch
        {
            0 => new Electronics
            {
                Id = Guid.NewGuid(),
                Name = $"Electronics Item {random.Next(1000)}",
                Weight = random.Next(1, 500),
                Price = (decimal)(random.NextDouble() * 1000 + random.Next(1, 1000)),
                SerialNumber = $"SN{random.Next(100000)}",
                WarrantyMonths = random.Next(12, 36),
                PowerWatts = (float)(random.NextDouble() * 100 + 10)
            },
            1 => new Chemicals
            {
                Id = Guid.NewGuid(),
                Name = $"Chemical Item {random.Next(1000)}",
                Weight = random.Next(1, 500),
                Price = (decimal)(random.NextDouble() * 500 + random.Next(1, 500)),
                Hazard = (HazardClass)random.Next(0, 4),
                ExpirationDate = DateTime.UtcNow.AddDays(random.Next(30, 365)),
                VolumeLiters = (float)(random.NextDouble() * 100 + 1)
            },
            2 => new Furniture
            {
                Id = Guid.NewGuid(),
                Name = $"Furniture Item {random.Next(1000)}",
                Weight = random.Next(1, 1000),
                Price = (decimal)(random.NextDouble() * 100 + random.Next(1, 100)),
                Material = $"Material {random.Next(1, 5)}",
                Dimensions = new Dimension(
                    Length: (float)(random.NextDouble() * 200 + 50),
                    Width: (float)(random.NextDouble() * 200 + 50),
                    Height: (float)(random.NextDouble() * 200 + 50)
                ),
                RequiresAssembly = random.Next(0, 2) == 1
            },
            _ => throw new InvalidOperationException("Invalid item type generated.")
        };
    }
}