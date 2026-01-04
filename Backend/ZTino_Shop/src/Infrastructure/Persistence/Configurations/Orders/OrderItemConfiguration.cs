using Domain.Models.Orders;

namespace Infrastructure.Persistence.Configurations.Orders
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Id)
                .ValueGeneratedOnAdd();

            builder.Property(oi => oi.OrderId)
                .IsRequired();

            builder.Property(oi => oi.ProductVariantId)
                .IsRequired();

            builder.Property(oi => oi.ProductId)
                .IsRequired();

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(oi => oi.ColorName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(oi => oi.SizeName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(oi => oi.ThumbnailUrl)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(oi => oi.CategoryId)
                .IsRequired();

            builder.Property(oi => oi.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.TotalLineAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(oi => oi.OrderId)
                .HasDatabaseName("IX_OrderItems_OrderId");

            builder.HasIndex(oi => oi.ProductId)
                .HasDatabaseName("IX_OrderItems_ProductId");

            builder.HasIndex(oi => oi.CategoryId)
                .HasDatabaseName("IX_OrderItems_CategoryId");
        }
    }
}
