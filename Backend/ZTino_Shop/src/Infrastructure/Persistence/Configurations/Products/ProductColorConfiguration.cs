using Domain.Models.Products;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Products
{
    internal class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            builder.ToTable("ProductColors", SchemaNames.Catalog);

            builder.HasKey(pc => pc.Id);

            builder.HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColors)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pc => pc.Color)
                .WithMany(c => c.ProductColors)
                .HasForeignKey(pc => pc.ColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pc => new { pc.ProductId, pc.ColorId }).IsUnique();
        }
    }
}
