using Domain.Constants;
using Domain.Models.Finances;

namespace Domain.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; } = default!;

        public Guid? UserId { get; set; }
        public string CustomerName { get; set; } = default!;
        public string CustomerPhone { get; set; } = default!;
        public string? CustomerEmail { get; set; }

        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }

        public int? PromotionId { get; set; }
        public string? PromotionName { get; set; }
        public string? VoucherCode { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = OrderStatus.Pending;
        public string PaymentStatus { get; set; } = Constants.PaymentStatus.Pending;
        public string PaymentMethod { get; set; } = Constants.PaymentMethod.COD;

        public string? Note { get; set; }
        public string? CancelReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedByIp { get; set; }

        public OrderAddress ShippingAddress { get; set; } = default!;
        public Guid? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<OrderPayment> Payments { get; set; } = new List<OrderPayment>();
        public ICollection<OrderStatusHistory> OrderHistory { get; set; } = new List<OrderStatusHistory>();
    }
}