namespace Application.Features.Orders.v1.DTOs
{
    public class OrderLookupItemDto
    {
        public string ProductName { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public string ColorName { get; set; } = default!;
        public string SizeName { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalLineAmount { get; set; }
    }
}
