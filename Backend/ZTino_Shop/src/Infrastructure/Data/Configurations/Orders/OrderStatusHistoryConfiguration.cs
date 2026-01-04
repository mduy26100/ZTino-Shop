using Domain.Models.Orders;

namespace Infrastructure.Data.Configurations.Orders
{
    internal class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.ToTable("OrderStatusHistories");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                .ValueGeneratedOnAdd();

            builder.Property(h => h.OrderId)
                .IsRequired();

            builder.Property(h => h.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(h => h.Note)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(h => h.ChangedBy)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(h => h.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(h => h.Order)
                .WithMany(o => o.OrderHistory)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(h => new { h.OrderId, h.CreatedAt })
                .HasDatabaseName("IX_OrderStatusHistories_OrderId_CreatedAt");

            builder.HasIndex(h => h.Status)
                .HasDatabaseName("IX_OrderStatusHistories_Status");
        }
    }
}
