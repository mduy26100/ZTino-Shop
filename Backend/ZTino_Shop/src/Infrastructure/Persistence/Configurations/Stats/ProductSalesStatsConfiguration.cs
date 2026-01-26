using Domain.Models.Stats;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Stats
{
    internal class ProductSalesStatsConfiguration : IEntityTypeConfiguration<ProductSalesStats>
    {
        public void Configure(EntityTypeBuilder<ProductSalesStats> builder)
        {
            builder.ToTable("ProductSalesStats", SchemaNames.Analytics);

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.ProductId)
                .IsRequired();

            builder.Property(s => s.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.TotalSoldQuantity)
                .IsRequired();

            builder.Property(s => s.TotalRevenue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.LastSoldAt)
                .IsRequired();

            builder.HasIndex(s => s.ProductId)
                .IsUnique()
                .HasDatabaseName("IX_ProductSalesStats_ProductId");

            builder.HasIndex(s => s.TotalSoldQuantity)
                .HasDatabaseName("IX_ProductSalesStats_TotalSoldQuantity");

            builder.HasIndex(s => s.TotalRevenue)
                .HasDatabaseName("IX_ProductSalesStats_TotalRevenue");
        }
    }
}
