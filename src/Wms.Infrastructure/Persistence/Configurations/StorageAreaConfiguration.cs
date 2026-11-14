using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class StorageAreaConfiguration:IEntityTypeConfiguration<StorageArea>
{
    public void Configure(EntityTypeBuilder<StorageArea> builder)
    {
        builder.ToTable("Wms_StorageArea");
        builder.HasKey(e => e.Id);
        builder.Property(e=>e.Code).IsRequired().HasMaxLength(50);
        builder.Property(e=>e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e=>e.Type).IsRequired().HasMaxLength(50);
        
        // 外键关系：一个库区属于一个仓库
        builder.HasOne(e => e.Warehouse)
            .WithMany()
            .HasForeignKey(e => e.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除，避免误删仓库时删除所有库区
        
        builder.HasIndex(e=>new{e.WarehouseId, e.Code}).IsUnique();
    }
}