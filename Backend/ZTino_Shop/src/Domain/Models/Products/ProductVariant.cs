namespace Domain.Models.Products
{
    public class ProductVariant
    {
        public int Id { get; set; }

        public int ProductColorId { get; set; }
        public int SizeId { get; set; }

        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public byte[] RowVersion { get; set; } = default!;

        public ProductColor ProductColor { get; set; } = default!;
        public Size Size { get; set; } = default!;

        public bool CanFulfill(int requestedQuantity)
            => IsActive && StockQuantity >= requestedQuantity;

        public void DeductStock(int quantity)
        {
            if (!CanFulfill(quantity))
                throw new InvalidOperationException(
                    $"Cannot deduct {quantity} from variant {Id}. Available: {StockQuantity}");
            StockQuantity -= quantity;
        }

        public void EnsureActive()
        {
            if (!IsActive)
                throw new InvalidOperationException($"ProductVariant {Id} is not active.");
        }
    }
}

