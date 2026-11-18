using Microsoft.EntityFrameworkCore;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence;

public class WmsDbContext:DbContext
{
    public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options)
    {
    }
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<StorageArea> StorageAreas => Set<StorageArea>();
    public DbSet<StorageLocation> StorageLocations => Set<StorageLocation>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<InboundOrder> InboundOrders => Set<InboundOrder>();
    public DbSet<InboundOrderDetail> InboundOrderDetails => Set<InboundOrderDetail>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<OutboundOrder> OutboundOrders => Set<OutboundOrder>();
    public DbSet<OutboundOrderDetail> OutboundOrderDetails => Set<OutboundOrderDetail>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WmsDbContext).Assembly);
    }
}