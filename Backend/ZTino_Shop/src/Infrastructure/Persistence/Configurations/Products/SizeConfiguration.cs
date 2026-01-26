using Domain.Models.Products;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Products
{
    internal class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.ToTable("Sizes", SchemaNames.Catalog);

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Description)
                .HasMaxLength(200);

            builder.HasIndex(s => s.Name).IsUnique();
        }
    }
}
