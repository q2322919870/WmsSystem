using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class OutboundOrderConfiguration:IEntityTypeConfiguration<OutboundOrder>
{
    public void Configure(EntityTypeBuilder<OutboundOrder> builder)
    {
        builder.ToTable("Wms_OutboundOrder");
        builder.HasKey(e => e.Id);
        builder.Property(e=>e.OrderNo).HasMaxLength(50).IsRequired();
        builder.Property(e=>e.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(e=>e.Status).HasMaxLength(20).IsRequired();
        
        builder.HasIndex(e=>e.OrderNo).IsUnique();
        builder.HasOne(e=>e.Warehouse).WithMany().HasForeignKey(e=>e.WarehouseId);
        builder.HasMany(e => e.Details)
            .WithOne(d => d.Order)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Cascade);// 配置一对多关系，出库单与出库单明细之间的关系，删除出库单时级联删除明细
    }
}