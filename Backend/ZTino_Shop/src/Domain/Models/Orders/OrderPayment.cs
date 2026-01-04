using Domain.Constants;

namespace Domain.Models.Orders
{
    public class OrderPayment
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }

        public string Method { get; set; } = PaymentMethod.COD;
        public string Status { get; set; } = PaymentStatus.Pending;

        public decimal Amount { get; set; }

        public string? TransactionId { get; set; }
        public string? PaymentGatewayResponse { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Order Order { get; set; } = default!;
    }
}