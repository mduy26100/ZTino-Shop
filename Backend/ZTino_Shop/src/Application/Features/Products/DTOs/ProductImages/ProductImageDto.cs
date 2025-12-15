namespace Application.Features.Products.DTOs.ProductImages
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }
}
