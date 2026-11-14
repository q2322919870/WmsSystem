using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class StorageLocationConfiguration:IEntityTypeConfiguration<StorageLocation>
{
    public void Configure(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.ToTable("Wms_StorageLocation");
        builder.HasKey(sl => sl.Id);
        builder.Property(sl=>sl.Code).HasMaxLength(50).IsRequired();
        builder.Property(sl=>sl.Name).HasMaxLength(100).IsRequired();
        builder.Property(sl=>sl.Status).HasMaxLength(20).IsRequired();
        
        builder.HasOne(e=>e.StorageArea)
            .WithMany()
            .HasForeignKey(e=>e.StorageAreaId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(e=>new{e.StorageAreaId,e.Code}).IsUnique();
    }
}