namespace Application.Features.Orders.v1.DTOs
{
    public class CreateOrderResponseDto
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; } = default!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!;
        public string PaymentMethod { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
