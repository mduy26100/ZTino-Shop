using Domain.Models.Products;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Products
{
    internal class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants", SchemaNames.Catalog);

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.StockQuantity)
                .IsRequired();

            builder.Property(v => v.IsActive)
                .HasDefaultValue(true);

            builder.Property(v => v.RowVersion)
                .IsRowVersion();

            builder.HasOne(v => v.ProductColor)
                .WithMany(pc => pc.ProductVariants)
                .HasForeignKey(v => v.ProductColorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Size)
                .WithMany(s => s.ProductVariants)
                .HasForeignKey(v => v.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(v => new { v.ProductColorId, v.SizeId }).IsUnique();
        }
    }
}
