using Domain.Models.Products;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Products
{
    internal class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages", SchemaNames.Catalog);

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pi => pi.IsMain)
                .HasDefaultValue(false);

            builder.Property(pi => pi.DisplayOrder)
                .HasDefaultValue(0);

            builder.HasOne(pi => pi.ProductColor)
                .WithMany(pc => pc.Images)
                .HasForeignKey(pi => pi.ProductColorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pi => new { pi.ProductColorId, pi.IsMain })
                .IsUnique()
                .HasFilter("[IsMain] = 1");
        }
    }
}
