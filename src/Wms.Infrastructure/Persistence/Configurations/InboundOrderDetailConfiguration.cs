using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class InboundOrderDetailConfiguration:IEntityTypeConfiguration<InboundOrderDetail>
{
    public void Configure(EntityTypeBuilder<InboundOrderDetail> builder)
    {
        builder.ToTable("Wms_InboundOrderDetail");
        builder.HasKey(d => d.Id);
        builder.HasOne(d=>d.Material)
            .WithMany()
            .HasForeignKey(d=>d.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);
        //Restrict是指当InboundOrderDetail表中的MaterialId列引用的Material表中的记录被删除时，不允许删除该Material记录，从而避免了数据的不一致性。
        builder.HasOne(d=>d.Location)
            .WithMany()
            .HasForeignKey(d=>d.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}