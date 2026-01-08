namespace Application.Features.Orders.v1.DTOs
{
    public class UpdateOrderDto
    {
        public Guid OrderId { get; set; }
        public string NewStatus { get; set; } = default!;
        public string? Note { get; set; }
        public string? CancelReason { get; set; }
    }
}