using Domain.Models.Products;

namespace Domain.Models.Carts
{
    public class CartItem
    {
        public int Id { get; set; }
        public Guid CartId { get; set; }

        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public Cart Cart { get; set; } = default!;

        public ProductVariant ProductVariant { get; set; } = default!;
    }
}