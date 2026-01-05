namespace Application.Features.Orders.v1.DTOs
{
    public class ShippingAddressDto
    {
        public string RecipientName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string City { get; set; } = default!;
    }
}