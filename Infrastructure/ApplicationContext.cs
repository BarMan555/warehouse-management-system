using AsyncWarehouse.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncWarehouse.Infrastructure;

public class ApplicationContext : DbContext
{
    public DbSet<Pallet> Pallets { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
}