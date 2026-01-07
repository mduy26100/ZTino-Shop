namespace Application.Features.Orders.v1.DTOs
{
    public class OrderLookupResponseDto
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!;
        public string PaymentMethod { get; set; } = default!;

        public string CustomerName { get; set; } = default!;
        public string CustomerPhone { get; set; } = default!;
        public string ShippingAddress { get; set; } = default!;
        public string? Note { get; set; }

        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderLookupItemDto> Items { get; set; } = new();
        public List<OrderLookupHistoryDto> Histories { get; set; } = new();
    }
}
