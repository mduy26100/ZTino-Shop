using Domain.Constants;
using Domain.Models.Finances;
using Domain.Models.Products;

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

        public static Order Create(
            Guid? userId,
            string customerName,
            string customerPhone,
            string? customerEmail,
            string? note,
            IEnumerable<(ProductVariant Variant, int Quantity)> variantsWithQuantity)
        {
            var orderItems = variantsWithQuantity
                .Select(x => OrderItem.CreateFromVariant(x.Variant, x.Quantity))
                .ToList();

            var subTotal = orderItems.Sum(oi => oi.TotalLineAmount);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderCode = GenerateOrderCode(),
                UserId = userId,
                CustomerName = customerName,
                CustomerPhone = customerPhone,
                CustomerEmail = customerEmail,
                SubTotal = subTotal,
                ShippingFee = 0,
                DiscountAmount = 0,
                TaxAmount = 0,
                TotalAmount = subTotal,
                Status = OrderStatus.Pending,
                PaymentStatus = Constants.PaymentStatus.Pending,
                PaymentMethod = Constants.PaymentMethod.COD,
                Note = note,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
                order.OrderItems.Add(item);
            }

            order.OrderHistory.Add(OrderStatusHistory.CreateInitial(order.Id));

            return order;
        }

        public void SetShippingAddress(
            string recipientName,
            string phoneNumber,
            string street,
            string ward,
            string district,
            string city)
        {
            ShippingAddress = new OrderAddress
            {
                OrderId = Id,
                RecipientName = recipientName,
                PhoneNumber = phoneNumber,
                Street = street,
                Ward = ward,
                District = district,
                City = city
            };
        }

        private static string GenerateOrderCode()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var randomPart = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
            return $"ORD-{timestamp}-{randomPart}";
        }
    }
}