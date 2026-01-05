using Domain.Constants;

namespace Domain.Models.Orders
{
    public class OrderStatusHistory
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }

        public string Status { get; set; } = OrderStatus.Pending; 
        public string? Note { get; set; }

        public string? ChangedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Order Order { get; set; } = default!;

        public static OrderStatusHistory CreateInitial(Guid orderId)
        {
            return new OrderStatusHistory
            {
                OrderId = orderId,
                Status = OrderStatus.Pending,
                Note = "Order created.",
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}