using Domain.Models.Carts;

namespace Infrastructure.Data.Configurations.Carts
{
    internal class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .ValueGeneratedOnAdd();

            builder.Property(ci => ci.CartId)
                .IsRequired();

            builder.Property(ci => ci.ProductVariantId)
                .IsRequired();

            builder.Property(ci => ci.Quantity)
                .IsRequired();

            builder.Property(ci => ci.AddedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.ProductVariant)
                .WithMany()
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ci => new { ci.CartId, ci.ProductVariantId })
                .IsUnique()
                .HasDatabaseName("IX_CartItems_CartId_ProductVariantId");
        }
    }
}
