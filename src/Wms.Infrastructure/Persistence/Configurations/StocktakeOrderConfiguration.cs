using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class StocktakeOrderConfiguration:IEntityTypeConfiguration<StocktakeOrder>
{
    public void Configure(EntityTypeBuilder<StocktakeOrder> builder)
    {
        builder.ToTable("Wms_StocktakeOrder");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderNo).HasMaxLength(50).IsRequired();
        builder.Property(o=>o.OperatorName).HasMaxLength(100).IsRequired();
        builder.Property(o=>o.Status).HasMaxLength(20).IsRequired();
        builder.HasIndex(o => o.OrderNo).IsUnique();
        builder.HasOne(o=>o.Warehouse)
            .WithMany()
            .HasForeignKey(o=>o.WarehousId)
            .OnDelete(DeleteBehavior.Restrict);// 仓库删除不允许删除盘点单
        builder.HasMany(o=>o.Details)
            .WithOne(d=>d.Order)
            .HasForeignKey(d=>d.OrderId)
            .OnDelete(DeleteBehavior.Cascade);// 盘点单删除则明细也删除
    }
}