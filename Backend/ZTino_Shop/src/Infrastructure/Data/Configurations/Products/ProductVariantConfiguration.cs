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

            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(v => v.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Color>()
                   .WithMany()
                   .HasForeignKey(v => v.ColorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Size>()
                   .WithMany()
                   .HasForeignKey(v => v.SizeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(v => new { v.ProductId, v.ColorId, v.SizeId }).IsUnique();
        }
    }
}