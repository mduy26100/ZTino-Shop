using Domain.Models.Products;

namespace Infrastructure.Data.Configurations.Products
{
    internal class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(pi => pi.IsMain)
                   .HasDefaultValue(false);

            builder.Property(pi => pi.DisplayOrder)
                   .HasDefaultValue(0);

            builder.HasOne<ProductVariant>()
                   .WithMany()
                   .HasForeignKey(pi => pi.ProductVariantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pi => new { pi.ProductVariantId, pi.IsMain }).IsUnique()
                   .HasFilter("[IsMain] = 1");
        }
    }
}
