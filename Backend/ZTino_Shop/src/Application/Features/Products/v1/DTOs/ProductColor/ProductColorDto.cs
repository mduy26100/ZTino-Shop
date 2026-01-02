using Application.Features.Products.v1.DTOs.Colors;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.DTOs.ProductVariants;

namespace Application.Features.Products.v1.DTOs.ProductColor
{
    public class ProductColorDto
    {
        public int Id { get; set; }
        public ColorDto Color { get; set; } = default!;
        public List<ProductImageDto> Images { get; set; } = new();
        public List<ProductVariantDto> Variants { get; set; } = new();
    }
}
