namespace Application.Features.Orders.v1.DTOs
{
    public class OrderSummaryDto
    {
        public Guid Id { get; set; }

        public string OrderCode { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = default!;

        public decimal TotalAmount { get; set; }

        public string PaymentStatus { get; set; } = default!;

        public int ItemCount { get; set; }
        public string FirstProductName { get; set; } = default!;
    }
}