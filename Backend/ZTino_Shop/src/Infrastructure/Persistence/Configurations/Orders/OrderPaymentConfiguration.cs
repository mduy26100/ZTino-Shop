using Domain.Models.Orders;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Orders
{
    internal class OrderPaymentConfiguration : IEntityTypeConfiguration<OrderPayment>
    {
        public void Configure(EntityTypeBuilder<OrderPayment> builder)
        {
            builder.ToTable("OrderPayments", SchemaNames.Sales);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.OrderId)
                .IsRequired();

            builder.Property(p => p.Method)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.TransactionId)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(p => p.PaymentGatewayResponse)
                .IsRequired(false)
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.OrderId)
                .HasDatabaseName("IX_OrderPayments_OrderId");

            builder.HasIndex(p => p.TransactionId)
                .HasDatabaseName("IX_OrderPayments_TransactionId");

            builder.HasIndex(p => p.Status)
                .HasDatabaseName("IX_OrderPayments_Status");
        }
    }
}
