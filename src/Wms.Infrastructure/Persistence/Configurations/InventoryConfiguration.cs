using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class InventoryConfiguration:IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Wms_Inventory");
        builder.HasKey(i => i.Id);
        
        // 唯一约束：同一库位同一物料只能有一行库存
        builder.HasIndex(i=>new{i.LocationId,i.MaterialId}).IsUnique();
        builder.HasOne(i=>i.Warehouse)
            .WithMany()
            .HasForeignKey(i=>i.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除
        builder.HasOne(i=>i.StorageLocation)
            .WithMany()
            .HasForeignKey(i=>i.LocationId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除
        builder.HasOne(i=>i.Material)
            .WithMany()
            .HasForeignKey(i=>i.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除
        
        builder.Ignore(i=>i.AvailableQty);// 忽略计算属性
        builder.Property(i=>i.RowVersion).IsRowVersion();// 乐观锁
    }
}