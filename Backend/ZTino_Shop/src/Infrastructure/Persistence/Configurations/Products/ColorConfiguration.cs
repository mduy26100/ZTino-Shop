using Domain.Models.Products;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Products
{
    internal class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Colors", SchemaNames.Catalog);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}
