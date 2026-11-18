using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class OutboundOrderDetailConfiguration:IEntityTypeConfiguration<OutboundOrderDetail>
{
    public void Configure(EntityTypeBuilder<OutboundOrderDetail> builder)
    {
        builder.ToTable("Wms_OutboundOrderDetail");
        builder.HasKey(e=>e.Id);
        
        builder.HasOne(e=>e.Material).WithMany().HasForeignKey(e=>e.MaterialId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e=>e.Location).WithMany().HasForeignKey(e=>e.LocationId).OnDelete(DeleteBehavior.Restrict);
    }
}