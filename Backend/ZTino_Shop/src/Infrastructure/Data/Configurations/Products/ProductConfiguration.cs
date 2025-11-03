using Domain.Models.Products;

namespace Infrastructure.Data.Configurations.Products
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.Slug)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.BasePrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.MainImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne<Category>()
                   .WithMany()
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.Slug).IsUnique();
        }
    }
}