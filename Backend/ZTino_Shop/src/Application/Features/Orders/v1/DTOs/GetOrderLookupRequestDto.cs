namespace Application.Features.Orders.v1.DTOs
{
    public class GetOrderLookupRequestDto
    {
        public string OrderCode { get; set; } = default!;
        public string CustomerPhone { get; set; } = default!;
    }
}
