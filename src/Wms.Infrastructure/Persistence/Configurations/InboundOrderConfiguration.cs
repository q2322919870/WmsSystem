using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class InboundOrderConfiguration:IEntityTypeConfiguration<InboundOrder>
{
    public void Configure(EntityTypeBuilder<InboundOrder> builder)
    {
        builder.ToTable("Wms_InboundOrder");
        builder.HasKey(o => o.Id);
        builder.Property(o=>o.OrderNo).HasMaxLength(50).IsRequired();
        builder.Property(o=>o.SupplierName).HasMaxLength(200).IsRequired();
        builder.Property(o=>o.Status).HasMaxLength(20).IsRequired();
        builder.HasIndex(o=>o.OrderNo).IsUnique();
        
        builder.HasMany(o=>o.Details)
            .WithOne(d=>d.Order)
            .HasForeignKey(d=>d.OrderId)//这里的外键是给InboundOrderDetail表中的OrderId列设置的，它引用了InboundOrder表中的Id列，表示每个入库单明细都属于一个入库单。
            .OnDelete(DeleteBehavior.Cascade);//Cascade是指当InboundOrder表中的记录被删除时，所有与该记录相关联的InboundOrderDetail表中的记录也会被自动删除，从而保持数据的一致性。
        builder.HasOne(o=>o.Warehouse)
            .WithMany()
            .HasForeignKey(o=>o.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);// 禁止级联删除
    }
}