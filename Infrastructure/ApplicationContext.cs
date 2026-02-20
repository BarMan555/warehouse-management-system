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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<InventoryItem>()
            .HasDiscriminator<string>("ItemType")
            .HasValue<Chemicals>("Chemical")
            .HasValue<Electronics>("Electronics")
            .HasValue<Furniture>("Furniture");
    }
}