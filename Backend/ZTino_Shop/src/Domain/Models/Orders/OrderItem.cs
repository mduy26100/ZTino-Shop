namespace Domain.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }

        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;
        public string Sku { get; set; } = default!;

        public string ColorName { get; set; } = default!;
        public string SizeName { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalLineAmount { get; set; }

        public Order Order { get; set; } = default!;
    }
}