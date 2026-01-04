namespace Application.Features.Carts.v1.DTOs
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? MainImageUrl { get; set; }
        public string SizeName { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ItemTotal { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
