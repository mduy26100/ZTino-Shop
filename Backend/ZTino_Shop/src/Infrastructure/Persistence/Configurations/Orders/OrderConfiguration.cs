using Domain.Models.Orders;

namespace Infrastructure.Persistence.Configurations.Orders
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder.Property(o => o.OrderCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(o => o.UserId)
                .IsRequired(false);

            builder.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.CustomerPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(o => o.CustomerEmail)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(o => o.SubTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.ShippingFee)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.PromotionId)
                .IsRequired(false);

            builder.Property(o => o.PromotionName)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(o => o.VoucherCode)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(o => o.DiscountAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.TaxAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.Note)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(o => o.CancelReason)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(o => o.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(o => o.UpdatedAt)
                .IsRequired(false);

            builder.Property(o => o.CreatedByIp)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.HasOne(o => o.ShippingAddress)
                .WithOne(a => a.Order)
                .HasForeignKey<OrderAddress>(a => a.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderHistory)
                .WithOne(h => h.Order)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.OrderCode)
                .IsUnique()
                .HasDatabaseName("IX_Orders_OrderCode");

            builder.HasIndex(o => o.UserId)
                .HasDatabaseName("IX_Orders_UserId");

            builder.HasIndex(o => o.Status)
                .HasDatabaseName("IX_Orders_Status");

            builder.HasIndex(o => o.CreatedAt)
                .HasDatabaseName("IX_Orders_CreatedAt");

            builder.HasIndex(o => o.PaymentStatus)
                .HasDatabaseName("IX_Orders_PaymentStatus");
        }
    }
}
