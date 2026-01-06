namespace Application.Features.Carts.v1.DTOs
{
    public class CartDto
    {
        public Guid CartId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
    }
}
