namespace Application.Features.Products.v1.DTOs.ProductImages
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }
}
