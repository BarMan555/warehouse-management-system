using AsyncWarehouse.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncWarehouse.Infrastructure;

/// <summary>
/// Represents the database context for the AsyncWarehouse application, managing entities like Pallets and InventoryItems.
/// </summary>
public class ApplicationContext : DbContext
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="Pallet"/> entities.
    /// </summary>
    public DbSet<Pallet> Pallets { get; set; }
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="InventoryItem"/> entities.
    /// </summary>
    public DbSet<InventoryItem> InventoryItems { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Configures the schema needed for the model.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
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