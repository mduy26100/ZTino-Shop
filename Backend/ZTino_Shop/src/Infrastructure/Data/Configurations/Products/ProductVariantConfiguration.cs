using Domain.Models.Products;

namespace Infrastructure.Data.Configurations.Products
{
    internal class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.StockQuantity)
                .IsRequired();

            builder.Property(v => v.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(v => v.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Color)
                .WithMany(c => c.ProductVariants)
                .HasForeignKey(v => v.ColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Size)
                .WithMany(s => s.ProductVariants)
                .HasForeignKey(v => v.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(v => new { v.ProductId, v.ColorId, v.SizeId }).IsUnique();
        }
    }
}