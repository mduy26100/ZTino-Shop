namespace Application.Features.Carts.v1.DTOs
{
    public class UpsertCartDto
    {
        public Guid? CartId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
