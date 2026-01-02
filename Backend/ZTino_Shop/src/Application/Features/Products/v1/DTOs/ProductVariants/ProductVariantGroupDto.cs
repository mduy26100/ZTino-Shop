using Application.Features.Products.v1.DTOs.ProductImages;

namespace Application.Features.Products.v1.DTOs.ProductVariants
{
    public class ProductVariantGroupDto
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; } = default!;

        public List<ProductImageDto> Images { get; set; } = new();

        public List<ProductVariantOptionDto> Options { get; set; } = new();
    }
}
