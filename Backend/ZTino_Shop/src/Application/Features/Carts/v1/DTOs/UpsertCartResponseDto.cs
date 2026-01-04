namespace Application.Features.Carts.v1.DTOs
{
    public class UpsertCartResponseDto
    {
        public Guid CartId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
