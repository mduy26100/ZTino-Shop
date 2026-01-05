namespace Application.Features.Orders.v1.DTOs
{
    public class CreateOrderDto
    {
        public Guid CartId { get; set; }
        public List<int> SelectedCartItemIds { get; set; } = new();

        public string CustomerName { get; set; } = default!;
        public string CustomerPhone { get; set; } = default!;
        public string? CustomerEmail { get; set; }

        public ShippingAddressDto ShippingAddress { get; set; } = default!;

        public string? Note { get; set; }
    }
}
