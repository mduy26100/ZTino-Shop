namespace Application.Features.Orders.v1.DTOs
{
    public class OrderLookupHistoryDto
    {
        public string Status { get; set; } = default!;
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
