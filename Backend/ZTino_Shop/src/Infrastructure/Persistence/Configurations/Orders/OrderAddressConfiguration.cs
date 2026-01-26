using Domain.Models.Orders;
using Infrastructure.Persistence.Constants;

namespace Infrastructure.Persistence.Configurations.Orders
{
    internal class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
    {
        public void Configure(EntityTypeBuilder<OrderAddress> builder)
        {
            builder.ToTable("OrderAddresses", SchemaNames.Sales);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.OrderId)
                .IsRequired();

            builder.Property(a => a.RecipientName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(a => a.Ward)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.District)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Ignore(a => a.FullAddress);

            builder.HasOne(a => a.Order)
                .WithOne(o => o.ShippingAddress)
                .HasForeignKey<OrderAddress>(a => a.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.OrderId)
                .IsUnique()
                .HasDatabaseName("IX_OrderAddresses_OrderId");
        }
    }
}
