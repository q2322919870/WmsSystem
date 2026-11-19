using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class StocktakeOrderDetailConfiguration:IEntityTypeConfiguration<StocktakeOrderDetail>
{
    public void Configure(EntityTypeBuilder<StocktakeOrderDetail> builder)
    {
        builder.ToTable("Wms_StocktakeOrderDetail");
        builder.HasKey(d => d.Id);
        builder.HasOne(d=>d.Location)
            .WithMany()
            .HasForeignKey(d=>d.LocationId)
            .OnDelete(DeleteBehavior.Restrict);// 库位删除不允许删除盘点单明细
        builder.HasOne(d=>d.Material)
            .WithMany()
            .HasForeignKey(d=>d.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);// 物料删除不允许删除盘点单
    }
}