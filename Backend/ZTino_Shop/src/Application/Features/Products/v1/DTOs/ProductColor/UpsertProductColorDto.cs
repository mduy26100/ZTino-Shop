namespace Application.Features.Products.v1.DTOs.ProductColor
{
    public class UpsertProductColorDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
    }
}
