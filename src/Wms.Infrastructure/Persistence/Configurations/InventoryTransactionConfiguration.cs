using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class InventoryTransactionConfigurationL : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("Wms_InventoryTransaction");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TransactionType).HasMaxLength(20).IsRequired();
        builder.Property(t=>t.ReferenceNo).HasMaxLength(50).IsRequired();
        builder.Property(t=>t.OperatorName).HasMaxLength(100).IsRequired();
        
        // 索引：方便按物料或单号查询流水
        builder.HasIndex(t => t.MaterialId);
        builder.HasIndex(t=> t.ReferenceNo);
        
        builder.HasOne(t=>t.Warehouse)
            .WithMany()
            .HasForeignKey(t=>t.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除
        
        builder.HasOne(t=>t.Location)
            .WithMany()
            .HasForeignKey(t=>t.LocationId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除
        
        builder.HasOne(t=>t.Material)
            .WithMany()
            .HasForeignKey(t=>t.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除 
    }
}