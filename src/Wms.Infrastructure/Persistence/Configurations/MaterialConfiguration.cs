using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Domain.Entities;

namespace Wms.Infrastructure.Persistence.Configurations;

public class MaterialConfiguration:IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Wms_Material");
        builder.HasKey(m => m.Id);
        builder.Property(m=>m.Code).HasMaxLength(50).IsRequired();
        builder.Property(m=>m.Name).HasMaxLength(200).IsRequired();
        builder.Property(m=>m.Unit).HasMaxLength(20).IsRequired();
        builder.Property(m=>m.BarCode).HasMaxLength(100);
        builder.Property(m=>m.Category).HasMaxLength(100);
        builder.HasIndex(m=>m.Code).IsUnique();

        // 条形码索引（方便扫码查询）
        builder.HasIndex(m => m.BarCode);

    }
}